using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TankStealer
{
    class _GameState
    {
        public enum gameState
        {
            gameMenu,
            gameOptions,
            gamePlaying,
            gamePaused,
            gameLoad,
            gameExit
        };

        gameState current;

        public _GameState()
        {
            current = _GameState.gameMenu;
        }

        _GameState getGameState(_GameState gameState)
        {
            _GameState newState;


            return newState;
        }
    }
}
