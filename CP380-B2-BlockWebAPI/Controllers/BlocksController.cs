using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CP380_B1_BlockList.Models;
using CP380_B2_BlockWebAPI.Models;

namespace CP380_B2_BlockWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class BlocksController : ControllerBase
    {
        // TODO

        private readonly BlockList _blockList;

        public BlocksController(BlockList blockList)
        {
            _blockList = blockList;
        }


        [HttpGet("/Blocks")]
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


        [HttpGet("/Blocks/{hash}")]
        public ActionResult<Block> Get(string hash)
        {
            var block = _blockList.Chain.Where(tempBlock => tempBlock.Hash == hash).First();

            return block.Hash.Length > 0 ? block : NotFound();
        }

        [HttpGet("/Blocks/{hash}/Payloads")]
        public ActionResult<List<Payload>> GetPayload(string hash)
        {
            
            var block = _blockList.Chain.Where(tempBlock => tempBlock.Hash == hash);

            return block.Select(tmpBlock => tmpBlock.Data).First();
    }


    }
}
