using Microsoft.AspNetCore.Mvc;
using Ord_Common;
using Ord_Business;

namespace OrderizerAPI.Controllers
{
    public class OrderController : ControllerBase
    {
        private readonly Ord_BusinessServ _businessSer;
        public OrderController(Ord_BusinessServ businessSer)
        {
            _businessSer = businessSer;
        }

        [HttpPost("AddItem")]
        public IActionResult AddNewItem([FromBody] items item)
        {
            try
            {
                _businessSer.AddNewItem(item);
                return Ok(new { message = "Item added successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("GetAllItems")]
        public IActionResult GetAllItems()
        {
            try
            {
                var items = _businessSer.GetAllItems();
                return Ok(items);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("GetItemById/{id}")]
        public IActionResult GetItemById(int id)
        {
            try
            {
                var item = _businessSer.GetItemById(id);
                if (item == null)
                    return NotFound(new { message = "Item not found" });

                return Ok(item);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPut("UpdateItem")]
        public IActionResult UpdateItem([FromBody] items item)
        {
            try
            {
                _businessSer.UpdateItem(item);
                return Ok(new { message = "Item updated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete("DeleteItem/{id}")]
        public IActionResult DeleteItem(int id)
        {
            try
            {
                _businessSer.DeleteItem(id);
                return Ok(new { message = "Item deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("GetBestDeal/{itemName}")]
        public IActionResult GetBestDeal(string itemName)
        {
            try
            {
                var item = _businessSer.GetBestDeal(itemName);
                if (item == null)
                    return NotFound(new { message = "Item not found or no platforms available" });

                return Ok(item);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
        [HttpPut("send Email")]
        public void sendEmail(string Receiver, string subject, List<items> allItems)
        {
            _businessSer.sendEmail(Receiver, subject, allItems);
        }
    }
}
