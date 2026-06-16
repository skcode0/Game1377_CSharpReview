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

    //TODO: create struct


    private string[,] tileNames = { { "Dark Cave", "Mossy Tunnel", "Crystal Room", "Frozen Lake" },
                                    { "Bone Chamber", "Flooded Hall", "Iron Gate", "Green Meadow" },
                                    { "Goblin Den", "Armory", "Throne Room", "Poison Swamp" },
                                    { "City Ruins","Bottomless Pit", "Ancient Forest", "Rotting Catacomb" }
                                  };

    private TileType[,] tileTypes = {   { TileType.Empty, TileType.Item,  TileType.Empty, TileType.Teleporter},
                                        { TileType.Enemy, TileType.Teleporter, TileType.Blockade, TileType.Exit },
                                        { TileType.Blockade, TileType.Empty, TileType.Enemy, TileType.Item },
                                        { TileType.Teleporter, TileType.Empty, TileType.Teleporter, TileType.Item }
                                    };

    private string[,] tileDescriptions = {
                                                {
                                                    "A damp cave echoes with dripping water.",
                                                    "Soft moss carpets the narrow tunnel floor.",
                                                    "Glittering crystals reflect dancing light.",
                                                    "A frozen lake stretches into the darkness."
                                                },
                                                {
                                                    "Ancient bones litter the chamber.",
                                                    "Cold water floods the abandoned hall.",
                                                    "A rusted iron gate blocks the path.",
                                                    "Unexpected wildflowers sway in the meadow."
                                                },
                                                {
                                                    "Crude goblin markings cover the walls.",
                                                    "Dusty piles of weapons remain in the unlit armory.",
                                                    "A forgotten throne sits in silence.",
                                                    "Thick fumes rise from the poisonous swamp."
                                                },
                                                {
                                                    "Ruined buildings hint at a lost civilization.",
                                                    "The pit descends beyond sight.",
                                                    "Massive trees surround the ancient forest.",
                                                    "The catacomb reeks of decay and death."
                                                }
                                         };

    private bool[,] isRoomVisited = {
                                    { false, false, false, false },
                                    { false, false, false, false },
                                    { false, false, false, false },
                                    { false, false, false, false }
                              };

    private int playerRow = 0;
    private int playerCol = 0;
    private int playerHealth = 10;
    private int enemyDamage = 1;
    private int itemHealAmount = 2;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        OutputTileInformation();
        SetPlayerPosition(0, 0);
        Debug.Log("\n");
    }

    // Update is called once per frame
    void Update()
    {
        bool wasKeyPressed = HandleInput(out int newRow, out int newCol);

        if (!wasKeyPressed)
        {
            return;
        }

        SetPlayerPosition(newRow, newCol);

        if (!isRoomVisited[playerRow, playerCol])
        {
            OutputTileInformation();
            isRoomVisited[playerRow, playerCol] = true;
        }

        Debug.Log("\n");
    }

    /// <summary>
    /// Gives info about the current tile the player is in.
    /// </summary>
    private void OutputTileInformation()
    {
        Debug.Log("You are in: " + tileNames[playerRow, playerCol]);
        Debug.Log(tileDescriptions[playerRow, playerCol]);
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
        if (CheckIfNewPositionInTileBounds(newRow, newCol))
        {
            playerRow = newRow;
            playerCol = newCol;
        }
        else
        {
            Debug.Log("Can't go that way");
        }
    }

    /// <summary>
    /// Determine if the new row and column position are within the bounds of the tiles or if the path is not blocked
    /// </summary>
    /// <param name="newRow">new row position</param>
    /// <param name="newCol">new column position</param>
    /// <returns>True if it is within the bounds, false if not</returns>
    private bool CheckIfNewPositionInTileBounds(int newRow, int newCol)
    {
        return (newRow >= 0 && newRow < tileNames.GetLength(0)) && (newCol >= 0 && newCol < tileNames.GetLength(1) && tileTypes[newRow, newCol] != TileType.Blockade);
    }

    /// <summary>
    /// Handles the player's input and sets potential new position in the tileNames array. If 'H' is pressed, it outputs description of the current room.
    /// </summary>
    /// <param name="newRow">new row position</param>
    /// <param name="newCol">new column position</param>
    /// <returns>True if an input was pressed, false if not</returns>
    private bool HandleInput(out int newRow, out int newCol)
    {
        bool hasPressedKey = true;
        newRow = playerRow;
        newCol = playerCol;
        
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
            Debug.Log($"You pressed {KeyCode.H}.");
            OutputTileInformation();
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
