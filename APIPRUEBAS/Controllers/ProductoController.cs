using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;
using APIPRUEBAS.Models;
using Microsoft.Identity.Client;
using System;

using Microsoft.AspNetCore.Cors;

namespace APIPRUEBAS.Controllers
{
    [EnableCors("ReglasCors")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        public readonly DbapiContext _dbapiContext;

        public ProductoController(DbapiContext _context)
        {
            _dbapiContext = _context;
        }

        [HttpGet]
        [Route("Lista")]
        public IActionResult Lista()
        {
            List<Producto> lista = new List<Producto>();

            try
            {
                lista = _dbapiContext.Productos.Include(c => c.oCategoria).ToList();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "0k", response = lista });
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message, response = lista });
            }
        }

        [HttpGet]
        [Route("Obtener/{idProducto:int}")]
        public IActionResult Obtener(int idProducto)
        {
            Producto oProducto = _dbapiContext.Productos.Find(idProducto);

            if(oProducto == null)
            {
                return BadRequest("Producto no encontrado");
            }

            try
            {
                oProducto = _dbapiContext.Productos.Include(c => c.oCategoria)
                                         .Where(p => p.IdProducto == idProducto)
                                         .FirstOrDefault();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "0k", response = oProducto });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message, response = oProducto });
            }
        }

        [HttpPost]
        [Route("Guardar")]
        public IActionResult Guardar([FromBody] Producto objeto)
        {
            try
            {
                _dbapiContext.Productos.Add(objeto);
                _dbapiContext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "0k" });

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message });
            }
        }

        [HttpPut]
        [Route("Editar")]
        public IActionResult Editar([FromBody] Producto objeto)
        {
            Producto oProducto = _dbapiContext.Productos.Find(objeto.IdProducto);

            if (oProducto == null)
            {
                return BadRequest("Producto no encontrado");
            }

            try
            {
                oProducto.CodigoBarra = objeto.CodigoBarra is null ? oProducto.CodigoBarra : objeto.CodigoBarra;
                oProducto.Descripcion = objeto.Descripcion is null ? oProducto.Descripcion : objeto.Descripcion;
                oProducto.Marca = objeto.Marca is null ? oProducto.Marca : objeto.Marca;
                oProducto.IdCategoria = objeto.IdCategoria is null ? oProducto.IdCategoria : objeto.IdCategoria;
                oProducto.Precio = objeto.Precio is null ? oProducto.Precio : objeto.Precio;

                _dbapiContext.Productos.Update(oProducto);
                _dbapiContext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "0k" });

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message });
            }
        }

        [HttpDelete]
        [Route("Eliminar/{idProducto:int}")]
        public IActionResult Eliminar(int idProducto)
        {
            Producto oProducto = _dbapiContext.Productos.Find(idProducto);

            if (oProducto == null)
            {
                return BadRequest("Producto no encontrado");
            }

            try
            {
                _dbapiContext.Productos.Remove(oProducto);
                _dbapiContext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "0k" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message });
            }
        }
    }
}
