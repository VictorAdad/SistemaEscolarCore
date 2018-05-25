using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema.Models;

namespace Sistema.Controllers
{
    public class CategoriasController : Controller
    {
        private readonly SistemaContext _context;

        public CategoriasController(SistemaContext context)
        {
            _context = context;
        }

        // GET: Categorias
        public async Task<IActionResult> Index(string sortOrder, // Muestra el ordenamiento actual de las columnas
                                         string currentFilter, // Cadena de texto buscada por el usuario
                                         string searchString, // Nueva cadena de búsqueda
                                         int? page) // Número de página
        {
            ViewData["NombreSortParm"] = string.IsNullOrEmpty(sortOrder) ? "nombre_desc" : ""; // si está vacio, que se ordene por nombre de manera descendiente
            ViewData["DescripcionSortParam"] = sortOrder == "descripcion_asc" ? "descripcion_desc" : "descripcion_asc"; // si está vacío, ordena por desc. descendente, si tiene otra cosa, ascendente
            if (searchString != null) // Si el usuario ha realizado una búsqueda
            {
                page = 1; // Se vuelve a mostrar el resultado de la primera página
            }
            else
            {
                searchString = currentFilter; // El usuario no ha buscado nada nuevo, por lo que el filtro se mantiene en todas las páginas
            }
            ViewData["CurrentFilter"] = searchString; // Muestra la vista con la cadena de filtro actual. Sirve para mantener el filtro entre páginas
            ViewData["CurrentSort"] = sortOrder; // Muestra la vista con el "ordenamiento actual"

            var categorias = from s in _context.Categoria select s;

            if (!string.IsNullOrEmpty(searchString)) // Si no está vacío
            {
                categorias = categorias.Where(s => s.Nombre.Contains(searchString) || s.Descripcion.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "nombre_desc":
                    categorias = categorias.OrderByDescending(s => s.Nombre);
                    break;
                case "descripcion_desc":
                    categorias = categorias.OrderByDescending(s => s.Descripcion);
                    break;
                case "descripcion_asc":
                    categorias = categorias.OrderBy(s => s.Descripcion);
                    break;
                default:
                    categorias = categorias.OrderBy(s => s.Nombre);
                    break;
            }

            //return View(await _context.Categoria.ToListAsync());
            //return View(await categorias.AsNoTracking().ToListAsync());
            const int pageSize = 2; // No. de registros por página
            return View(await Paginacion<Categoria>.CreateAsync(categorias.AsNoTracking(), page ?? 1, pageSize)); // Devuelve las categorías a la vista, dependiendo del número de registros (pageSize)) y devuelve el valor de page para saber la página o bien "1" por defecto
        }

        // GET: Categorias/Details/5
        public async Task<IActionResult> Details(int? id) // Details muestra el detalle de un registro seleccionado (int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoria = await _context.Categoria
                .SingleOrDefaultAsync(m => m.CategoriaId == id);
            if (categoria == null)
            {
                return NotFound();
            }

            return View(categoria);
        }

        // GET: Categorias/Create
        public IActionResult Create() // Muestra la vista
        {
            return View();
        }

        // POST: Categorias/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoriaID,Nombre,Descripcion,Estado")] Categoria categoria) // Almacena lo que recibe del formulario de la vista create
        {
            if (ModelState.IsValid)
            {
                _context.Add(categoria);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(categoria);
        }

        // GET: Categorias/Edit/5
        public async Task<IActionResult> Edit(int? id) // Recibe el id del registro a editar
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoria = await _context.Categoria.SingleOrDefaultAsync(m => m.CategoriaId == id);
            if (categoria == null)
            {
                return NotFound();
            }
            return View(categoria);
        }

        // POST: Categorias/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Descripcion,Estado")] Categoria categoria) // Recibe todos los datos del formulario
        {
            if (id != categoria.CategoriaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(categoria);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoriaExists(categoria.CategoriaId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(categoria);
        }

        // GET: Categorias/Delete/5
        public async Task<IActionResult> Delete(int? id) // Borra ese registro
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoria = await _context.Categoria
                .SingleOrDefaultAsync(m => m.CategoriaId == id);
            if (categoria == null)
            {
                return NotFound();
            }

            return View(categoria);
        }

        // POST: Categorias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) // Confirma la eliminación
        {
            var categoria = await _context.Categoria.SingleOrDefaultAsync(m => m.CategoriaId == id);
            _context.Categoria.Remove(categoria);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoriaExists(int id)
        {
            return _context.Categoria.Any(e => e.CategoriaId == id);
        }
    }
}
