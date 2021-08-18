using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CP380_B1_BlockList.Models;
using CP380_B2_BlockWebAPI.Models;
using CP380_B2_BlockWebAPI.Services;


namespace CP380_B2_BlockWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class BlocksController : ControllerBase
    {
        // TODO

        private readonly BlockList _blockList;
        public List<Payload> payloadList { get; set; }

        public BlocksController(BlockList blockList)
        {
            _blockList = blockList;
        }


        [HttpGet]
        public ActionResult<List<BlockSummary>> Get()
        {
            List<Block> blocks = _blockList.Chain.ToList();
            //blocks.Add(new Block(DateTime.Now, null, new List<Payload>()));

            List<BlockSummary> blockSummaryList = new List<BlockSummary>();
            foreach (var block in blocks)
            {
                _blockList.AddBlock(block);
                blockSummaryList.Add(new BlockSummary()
                {
                    Hash = block.Hash,
                    PreviousHash = block.PreviousHash,
                    TimeStamp = block.TimeStamp,
                });
            }
            return blockSummaryList;

        }


        [HttpGet("/{hash}")]
        public ActionResult<Block> Get(string hash)
        {
            var block = _blockList.Chain.Where(tempBlock => tempBlock.Hash == hash).First();

            return block.Hash.Length > 0 ? block : NotFound();
        }

        [HttpGet("/{hash}/Payloads")]
        public ActionResult<List<Payload>> GetPayload(string hash)
        {

            var block = _blockList.Chain.Where(tempBlock => tempBlock.Hash == hash);

            return block.Select(tmpBlock => tmpBlock.Data).First();
        }

        [HttpPost]
        public void Post(Block block)
        {
            payloadList = new PendingPayloads().payloads;
            var tmpBlock = _blockList.Chain[_blockList.Chain.Count - 1];
            var block1 = new Block(tmpBlock.TimeStamp, tmpBlock.PreviousHash, payloadList);
            if (block1.CalculateHash() == block1.Hash)
            {
                _blockList.Chain.Add(block);

            }
            else
            {
                BadRequest();
            }

        }
    }
}
