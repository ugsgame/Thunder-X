
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MatrixEngine.Math;
using Thunder.GameLogic.Common;
using Thunder.GameLogic.Gaming.Actors;
using Thunder.GameLogic.Gaming.Actors.Players;
using MatrixEngine.Cocos2d;
using Thunder.GameLogic.Gaming.Actors.Drops;

namespace Thunder.GameLogic.Gaming
{
    public class PlayerSpawner
    {
        public enum PlayerID
        {
            Player1 = ActorID.Player1,
            Player2 = ActorID.Player2,
            Player3 = ActorID.Player3,
            Player4 = ActorID.Player4,
        };
        PlayerID playerID;

        List<Player> players = new List<Player>();  
        Player player;
        CCNode layer;

        //玩家公共属性
        SpawnInfo playerConfig = new SpawnInfo();

        public static PlayerSpawner Instanse;

        public PlayerSpawner()
        {
            playerConfig.resPath = ResID.Armatures_Player;
            playerConfig.armaName = "Player";
            playerConfig.speed = 1500.0f;
            playerConfig.HP = 200;
            playerConfig.critTime = GameData.Instance.CritTime;

            SpawnInfo player1Info = playerConfig.Clone();
            player1Info.animName = "player1";
            player1Info.name = "player1";
            player = new Player1(player1Info);
            players.Add(player);

            SpawnInfo player2Info = playerConfig.Clone();
            player2Info.animName = "player2";
            player2Info.name = "player2";
            player = new Player2(player2Info);
            players.Add(player);

            SpawnInfo player3Info = playerConfig.Clone();
            player3Info.animName = "player3";
            player3Info.name = "player3";
            player = new Player3(player3Info);
            players.Add(player); ;

            SpawnInfo player4Info = playerConfig.Clone();
            player4Info.animName = "player4";
            player4Info.name = "player4";
            player = new Player4(player4Info);
            players.Add(player);

            player = players[0];
        }

        public Player CurPlayer
        {
            get { return player; }
        }

        public PlayerID CurPlayerID
        {
            get { return playerID; }
        }

        public CCNode PlayerLayer
        {
            set 
            { 
                layer = value;
                playerConfig.position.X = layer.ContextSize.width / 2;
                playerConfig.position.Y = layer.ContextSize.height / 2;
            }
        }

        public virtual void SetCurPlayer(PlayerID playerId)
        {
            switch (playerId)
            {
                case PlayerID.Player1:
                    player = players[0];
                    break;
                case PlayerID.Player2:
                    player = players[1];
                    break;
                case PlayerID.Player3:
                    player = players[2];
                    break;
                case PlayerID.Player4:
                    player = players[3];
                    break;
                default:
                    break;
            }
            this.playerID = playerId;
        }

        public virtual List<Player> AllPlayer()
        {
            return players;
        }

        public virtual void ActivatePlayer()
        {
            player.OpenFire();
            //player.Postion = playerConfig.position;
            player.LevelManager = PlayingScene.Instance.LevelManager;
            DropManager.Instance.Player = player;
        }

        public bool IsShowing
        {
            set 
            {
                foreach (var item in players)
                {
                    item.IsShowing = value;
                }
            }
        }
    }
}
