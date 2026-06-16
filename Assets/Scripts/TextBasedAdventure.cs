using UnityEngine;

public class TextBasedAdventure : MonoBehaviour
{
    public enum TileType
    {
        Invalid,
        Empty,
        Item,
        Enemy,
        Exit,
        Blockade,
        Teleporter
    }

    #nullable enable
    private struct Tile
    {
        public string Name;
        public TileType Type;
        public string Description;
        public bool isTileVisited;
        public (int, int)? TeleportDestinationCoordinate; 
    }

    private Tile[,] dungeon =
    {
        {
            new Tile { Name = "Dark Cave", Type = TileType.Empty, isTileVisited = false, Description = "A damp cave echoes with dripping water." },
            new Tile { Name = "Mossy Tunnel", Type = TileType.Item, isTileVisited = false, Description = "Soft moss carpets the narrow tunnel floor." },
            new Tile { Name = "Crystal Room", Type = TileType.Empty, isTileVisited = false, Description = "Glittering crystals reflect dancing light." },
            new Tile { Name = "Frozen Lake", Type = TileType.Teleporter, isTileVisited = false, Description = "A frozen lake stretches into the darkness.", TeleportDestinationCoordinate = (3,2) }
        },
        {
            new Tile { Name = "Bone Chamber", Type = TileType.Enemy, isTileVisited = false, Description = "Ancient bones litter the chamber." },
            new Tile { Name = "Flooded Hall", Type = TileType.Teleporter, isTileVisited = false, Description = "Cold water floods the abandoned hall.", TeleportDestinationCoordinate = (3,0) },
            new Tile { Name = "Iron Gate", Type = TileType.Blockade, isTileVisited = false, Description = "A rusted iron gate blocks the path." },
            new Tile { Name = "Green Meadow", Type = TileType.Exit, isTileVisited = false, Description = "Unexpected wildflowers sway in the meadow." }
        },
        {
            new Tile { Name = "Goblin Den", Type = TileType.Blockade, isTileVisited = false, Description = "Crude goblin markings cover the walls." },
            new Tile { Name = "Armory", Type = TileType.Empty, isTileVisited = false, Description = "Dusty piles of weapons remain in the unlit armory." },
            new Tile { Name = "Throne Room", Type = TileType.Enemy, isTileVisited = false, Description = "A forgotten throne sits in silence." },
            new Tile { Name = "Poison Swamp", Type = TileType.Item, isTileVisited = false, Description = "Thick fumes rise from the poisonous swamp." }
        },
        {
            new Tile { Name = "City Ruins", Type = TileType.Teleporter, isTileVisited = false, Description = "Ruined buildings hint at a lost civilization.", TeleportDestinationCoordinate = (1,1) },
            new Tile { Name = "Bottomless Pit", Type = TileType.Empty, isTileVisited = false, Description = "The pit descends beyond sight." },
            new Tile { Name = "Ancient Forest", Type = TileType.Teleporter, isTileVisited = false, Description = "Massive trees surround the ancient forest.", TeleportDestinationCoordinate = (0,3) },
            new Tile { Name = "Rotting Catacomb", Type = TileType.Item, isTileVisited = false, Description = "The catacomb reeks of decay and death." }
        },
    };


    private int playerRow = 0;
    private int playerCol = 0;
    private int playerHealth = 10;
    private int enemyDamage = 1;
    private int itemHealAmount = 2;
    private bool isBlocked = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetPlayerPosition(0, 0);
        OutputTileName();
        Look();
        dungeon[playerRow, playerCol].isTileVisited = true;

        Debug.Log("\n");
    }

    // Update is called once per frame
    void Update()
    {
        bool wasKeyPressed = HandleInput(out int newRow, out int newCol, out bool isDescriptionKeyPressed);

        if (!wasKeyPressed)
        {
            return;
        }

        SetPlayerPosition(newRow, newCol);

        if (dungeon[playerRow, playerCol].Type == TileType.Teleporter)
        {
            (playerRow, playerCol) = dungeon[playerRow, playerCol].TeleportDestinationCoordinate.Value;
        }

        OutputTileName();

        if (isDescriptionKeyPressed)
        {
            Look();
        }

        if (!dungeon[playerRow, playerCol].isTileVisited)
        {
            Look();
            dungeon[playerRow, playerCol].isTileVisited = true;
        }

        Debug.Log("\n");
    }

    /// <summary>
    /// Gives name of the current tile the player is in.
    /// </summary>
    private void OutputTileName()
    {
        Debug.Log($"You are {(isBlocked ? "still ": "")}in: " + dungeon[playerRow, playerCol].Name);
    }

    /// <summary>
    /// Gives description of the current tile the player is in.
    /// </summary>
    private void Look()
    {
        Debug.Log(dungeon[playerRow, playerCol].Description);
    }

    private void EncounterEnemy()
    {
        PlayerTakeDamage(enemyDamage);
    }
    
    private void ItemPickup()
    {
        PlayerHeal(itemHealAmount);
    }

    private void PlayerHeal(int heal)
    {
        playerHealth += heal;
        Debug.Log("You get healed. Your health is now " + playerHealth);
    }

    private void PlayerTakeDamage(int damage)
    {
        playerHealth -= damage;
        Debug.Log("You get hit. Your health is now " + playerHealth);
        if (playerHealth <= 0)
        {
            playerHealth = 0;
            Debug.Log("You are dead");
        }
    }

    /// <summary>
    /// Sets the player position to a new row and column position
    /// </summary>
    /// <param name="newRow"></param>
    /// <param name="newCol"></param>
    private void SetPlayerPosition(int newRow, int newCol)
    {
        if (CheckIfPositionInValidTile(newRow, newCol))
        {
            playerRow = newRow;
            playerCol = newCol;
        }
        else
        {
            Debug.Log((isBlocked ? "The way is Blocked. ": "") + "Can't go that way.");
        }
    }

    /// <summary>
    /// Determine if the row and column position are within the bounds of the tiles or if the path is not blocked
    /// </summary>
    /// <param name="row"> row position</param>
    /// <param name="col"> column position</param>
    /// <returns>True if it is within valid bounds, false if not</returns>
    private bool CheckIfPositionInValidTile(int row, int col)
    {
        if ((row >= 0 && row < dungeon.GetLength(0)) && (col >= 0 && col < dungeon.GetLength(1)))
        {
            isBlocked = dungeon[row, col].Type == TileType.Blockade;

            return !isBlocked;
        }

        return false;
    }

    /// <summary>
    /// Handles the player's input and sets potential new position in the tileNames array. If 'H' is pressed, it outputs description of the current room.
    /// </summary>
    /// <param name="newRow">new row position</param>
    /// <param name="newCol">new column position</param>
    /// <returns>True if an input was pressed, false if not</returns>
    private bool HandleInput(out int newRow, out int newCol, out bool isDescriptionKeyPressed)
    {
        bool hasPressedKey = true;
        newRow = playerRow;
        newCol = playerCol;

        isDescriptionKeyPressed = false;
        
        if (Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log("You pressed " + KeyCode.D);
            newCol++;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("You pressed " + KeyCode.A);
            newCol--;
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log("You pressed " + KeyCode.W);
            newRow--;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("You pressed " + KeyCode.S);
            newRow++;
        }
        else if (Input.GetKeyDown(KeyCode.H))
        {
            Debug.Log($"You pressed {KeyCode.H}");
            isDescriptionKeyPressed = true;
        }
        else
        {
            hasPressedKey = false;
        }
        return hasPressedKey;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name=""></param>
    /// <param name=""></param>
    /// <returns></returns>
    

}
