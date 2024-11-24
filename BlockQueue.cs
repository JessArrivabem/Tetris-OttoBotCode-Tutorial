using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public class BlockQueue
    {
        private readonly Block[] blocks = new Block[]
        {
            new IBlock(),
            new JBlock(),
            new LBlock(),
            new OBlock(),
            new SBlock(),
            new TBlock(),
            new ZBlock(),
        };

        private readonly Random random = new Random();

        public Block NextBlock {  get; private set; } // property for the next block in the queue

        public BlockQueue ()// constructor initialize the next block with a random block
        {
            NextBlock = RandomBlock();
        }

        private Block RandomBlock () // method that returns the random block
        {
            return blocks [random.Next (blocks.Length)];
        }

        public Block GetAndUpdate() // returns the block and updates the property
        {
            Block block = NextBlock;

            do
            {
                NextBlock = RandomBlock ();
            }
            while (block.Id == NextBlock.Id);

            return block;
        }
    }
}
