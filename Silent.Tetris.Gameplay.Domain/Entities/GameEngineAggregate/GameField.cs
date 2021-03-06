﻿using System.Collections.Generic;
using System.Linq;
using Silent.Tetris.Contracts.Core;

namespace Silent.Tetris.Core.Engine
{
    public class GameField : FieldBase, IGameField
    {
        private IFigure _currentFigure;
        private IGround _ground;

        public GameField(Size size) : base(size)
        {
            Position groundDefaultPosition = Position.None;
            _ground = new GoundFigure(groundDefaultPosition, size);
        }

        public IFigure CurrentFigure => _currentFigure;

        public IGround Ground => _ground;

        public void SetCurrentFigure(IFigure currentFigure)
        {
            _currentFigure = currentFigure;
        }

        public void SetGround(IGround ground)
        {
            _ground = ground;
        }

        protected override IEnumerable<ISprite> GetSpriteCollection()
        {
            return new ISprite[] { Ground, CurrentFigure }.Where(x => x != null);
        }
    }
}
