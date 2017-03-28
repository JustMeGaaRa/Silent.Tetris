using Silent.Tetris.Contracts.Core;
using Silent.Tetris.Core.Figures;

namespace Silent.Tetris.Core.Letters
{
    public class LetterE : FigureBase
    {
        public LetterE() : base(Position.None, new[,]
        {
            { Color.Cyan, Color.Cyan, Color.Cyan },
            { Color.Cyan, Color.Transparent, Color.Transparent },
            { Color.Cyan, Color.Cyan, Color.Cyan },
            { Color.Cyan, Color.Transparent, Color.Transparent },
            { Color.Cyan, Color.Cyan, Color.Cyan }
        })
        {
        }

        public LetterE(Position position, Color[,] cells) : base(position, cells)
        {
        }
    }
}