using System;
using System.Collections.Generic;
using AI;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject virtualCameraOne;
    [SerializeField] private GameObject virtualCameraTwo;
    [SerializeField] private GameObject botPrefab;
    [SerializeField] private GameObject spawnPointsParentObject;
    [SerializeField] public LayerMask teamOneLayerMask;
    [SerializeField] public LayerMask teamTwoLayerMask;
    [SerializeField] public LayerMask teamThreeLayerMask;
    [SerializeField] private Skin[] skins;
    [SerializeField] private GameObject ak47;
    [SerializeField] private GameObject pistol;
    [SerializeField] private GameObject shotgun;
    [SerializeField] private InputActionAsset playerOneInputAction;
    [SerializeField] private InputActionAsset playerTwoInputAction;
    [SerializeField] private Color teamOneColor;
    [SerializeField] private Color teamTwoColor;
    [SerializeField] private Color teamThreeColor;
    [SerializeField] private Sprite playerShape;

    private int _teamOneSkinIndex;
    private int _teamTwoSkinIndex;
    private int _teamThreeSkinIndex;
    private List<Transform> _availableSpawnPoints = new();

    private GameObject _playerOne;
    private GameObject _playerTwo;

    private readonly Dictionary<string, EntityInfo> _entityInfos = new();

    public static GameManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void OnEntityDestroy(string id)
    {
        if (spawnPointsParentObject == null || virtualCameraOne == null || virtualCameraTwo == null)
            return;

        _availableSpawnPoints = new List<Transform>();
        foreach (Transform child in spawnPointsParentObject.transform)
        {
            _availableSpawnPoints.Add(child);
        }

        EntityInfo info = _entityInfos[id];
        if (info == null)
            return;

        if (info.IsBot)
            SpawnBot(GetLayerMaskIndex(info.LayerMask));
        else
            SpawnPlayer(GetLayerMaskIndex(info.LayerMask), info.IsPlayerOne ? virtualCameraOne : virtualCameraTwo);

        _entityInfos.Remove(id);
    }

    private void Start()
    {
        foreach (Transform child in spawnPointsParentObject.transform)
        {
            _availableSpawnPoints.Add(child);
        }

        List<int> skinIndices = new List<int>();
        for (int i = 0; i < skins.Length; i++) skinIndices.Add(i);
        ShuffleList(skinIndices);

        _teamOneSkinIndex = skinIndices[0];
        if (GlobalSettings.NumberOfTeams >= 2) _teamTwoSkinIndex = skinIndices[1];
        if (GlobalSettings.NumberOfTeams >= 3) _teamThreeSkinIndex = skinIndices[2];

        int totalPlayers = GlobalSettings.PlayersPerTeam;
        int totalTeams = GlobalSettings.NumberOfTeams;

        SpawnPlayer(1, virtualCameraOne);
        SpawnPlayer(2, virtualCameraTwo);

        for (int team = 1; team <= totalTeams; team++)
        {
            for (int i = 0; i < totalPlayers - (team <= 2 ? 1 : 0); i++) // Skip 1 player for first two teams
            {
                SpawnBot(team);
            }
        }
    }

    private void SpawnPlayer(int teamNumber, GameObject cameraObject)
    {
        Transform spawnPoint = GetRandomSpawnPoint();
        GameObject player = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        player.gameObject.name = $"Player Team {(teamNumber == 1 ? "One" : "Two")}";

        player.GetComponent<PlayerSkinManager>().SetSkin(skins[GetTeamSkinIndex(teamNumber)]);

        cameraObject.GetComponent<Cinemachine.CinemachineVirtualCamera>().Follow = player.transform;
        cameraObject.GetComponent<Cinemachine.CinemachineVirtualCamera>().LookAt = player.transform;

        string entityId = Guid.NewGuid().ToString();

        if (teamNumber == 1)
        {
            player.GetComponent<PlayerHealthController>().isPlayerOne = true;
            player.GetComponent<PlayerWeaponsController>().isPlayerOne = true;
            player.GetComponent<PlayerWeaponsController>()
                .SetNewTargetLayerMasks(new[] { teamTwoLayerMask, teamThreeLayerMask });
            player.gameObject.layer = GetFirstLayerIndex(teamOneLayerMask);
            player.GetComponent<PlayerInput>().actions = playerOneInputAction;
            player.GetComponent<MiniMapIconController>().SetColor(teamOneColor);
            player.GetComponent<MiniMapIconController>().SetShape(playerShape);
            player.GetComponent<IEntity>().AssignId(entityId);
            _entityInfos.Add(entityId, new EntityInfo(teamOneLayerMask, true, false));
            player.GetComponent<PlayerController>().AssignControllers(true);
            _playerOne = player;
        }
        else
        {
            player.GetComponent<PlayerHealthController>().isPlayerOne = false;
            player.GetComponent<PlayerWeaponsController>().isPlayerOne = false;
            player.GetComponent<PlayerWeaponsController>()
                .SetNewTargetLayerMasks(new[] { teamOneLayerMask, teamThreeLayerMask });
            player.gameObject.layer = GetFirstLayerIndex(teamTwoLayerMask);
            player.GetComponent<PlayerInput>().actions = playerTwoInputAction;
            player.GetComponent<MiniMapIconController>().SetColor(teamTwoColor);
            player.GetComponent<MiniMapIconController>().SetShape(playerShape);
            player.GetComponent<IEntity>().AssignId(entityId);
            _entityInfos.Add(entityId, new EntityInfo(teamTwoLayerMask, false, false));
            player.GetComponent<PlayerController>().AssignControllers(false);
            _playerTwo = player;
        }

        GiveRandomGun(player);
    }

    private void SpawnBot(int teamNumber)
    {
        Transform spawnPoint = GetRandomSpawnPoint();
        GameObject bot = Instantiate(botPrefab, spawnPoint.position, Quaternion.identity);
        string[] teamNames = { "One", "Two", "Three" };
        string botName = $"Bot Team {teamNames[teamNumber - 1]}";
        bot.gameObject.name = botName;
        bot.GetComponent<PlayerSkinManager>().SetSkin(skins[GetTeamSkinIndex(teamNumber)]);

        string entityId = Guid.NewGuid().ToString();

        if (teamNumber == 1)
        {
            bot.gameObject.layer = GetFirstLayerIndex(teamOneLayerMask);
            bot.GetComponent<BotWeaponsController>()
                .SetNewTargetLayerMasks(new[] { teamTwoLayerMask, teamThreeLayerMask });
            bot.GetComponent<AIAgent>()
                .SetNewTargetLayerMasks(new[] { teamTwoLayerMask, teamThreeLayerMask });
            bot.GetComponent<MiniMapIconController>().SetColor(teamOneColor);
            bot.GetComponent<IEntity>().AssignId(entityId);
            _entityInfos.Add(entityId, new EntityInfo(teamOneLayerMask, false, true));

            EnemyUI enemyUI = bot.GetComponent<EnemyUI>();
            enemyUI.AddTarget(_playerTwo.transform);
            enemyUI.SetName(botName);
        }
        else if (teamNumber == 2)
        {
            bot.gameObject.layer = GetFirstLayerIndex(teamTwoLayerMask);
            bot.GetComponent<BotWeaponsController>()
                .SetNewTargetLayerMasks(new[] { teamOneLayerMask, teamThreeLayerMask });
            bot.GetComponent<AIAgent>()
                .SetNewTargetLayerMasks(new[] { teamOneLayerMask, teamThreeLayerMask });
            bot.GetComponent<MiniMapIconController>().SetColor(teamTwoColor);
            bot.GetComponent<IEntity>().AssignId(entityId);
            _entityInfos.Add(entityId, new EntityInfo(teamTwoLayerMask, false, true));

            EnemyUI enemyUI = bot.GetComponent<EnemyUI>();
            enemyUI.AddTarget(_playerOne.transform);
            enemyUI.SetName(botName);
        }
        else if (teamNumber == 3)
        {
            bot.gameObject.layer = GetFirstLayerIndex(teamThreeLayerMask);
            bot.GetComponent<BotWeaponsController>()
                .SetNewTargetLayerMasks(new[] { teamTwoLayerMask, teamOneLayerMask });
            bot.GetComponent<AIAgent>()
                .SetNewTargetLayerMasks(new[] { teamTwoLayerMask, teamOneLayerMask });
            bot.GetComponent<MiniMapIconController>().SetColor(teamThreeColor);
            bot.GetComponent<IEntity>().AssignId(entityId);
            _entityInfos.Add(entityId, new EntityInfo(teamThreeLayerMask, false, true));

            EnemyUI enemyUI = bot.GetComponent<EnemyUI>();
            enemyUI.AddTarget(_playerOne.transform);
            enemyUI.AddTarget(_playerTwo.transform);
            enemyUI.SetName(botName);
        }

        GiveRandomGun(bot);
    }

    private int GetTeamSkinIndex(int teamNumber)
    {
        return teamNumber switch
        {
            1 => _teamOneSkinIndex,
            2 => _teamTwoSkinIndex,
            3 => _teamThreeSkinIndex,
            _ => 0
        };
    }

    private Transform GetRandomSpawnPoint()
    {
        if (_availableSpawnPoints.Count == 0)
        {
            Debug.LogError("No available spawn points left!");
            return null;
        }

        int index = Random.Range(0, _availableSpawnPoints.Count);
        Transform chosen = _availableSpawnPoints[index];
        _availableSpawnPoints.RemoveAt(index);
        return chosen;
    }

    private void GiveRandomGun(GameObject character)
    {
        (GameObject gun, bool isGun) = GetRandomGun();
        if (character.GetComponent<PlayerWeaponsController>() != null)
            character.GetComponent<PlayerWeaponsController>().SetNewWeapon(isGun, gun);
        else if (character.GetComponent<BotWeaponsController>() != null)
            character.GetComponent<BotWeaponsController>().SetNewWeapon(isGun, gun);
    }

    private (GameObject gun, bool isGun) GetRandomGun()
    {
        GameObject[] guns = { ak47, pistol, shotgun };
        GameObject selected = guns[Random.Range(0, guns.Length)];

        bool isGun = selected != pistol;

        return (selected, isGun);
    }

    private void ShuffleList(List<int> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }

    private int GetFirstLayerIndex(LayerMask mask)
    {
        for (int i = 0; i < 32; i++)
        {
            if ((mask.value & (1 << i)) != 0)
                return i;
        }

        return -1;
    }

    private int GetLayerMaskIndex(LayerMask layerMask)
    {
        if (layerMask == teamOneLayerMask) return 1;
        if (layerMask == teamTwoLayerMask) return 2;
        if (layerMask == teamThreeLayerMask) return 3;

        return -1;
    }
}
