using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

using Model;
using INFRAESTRUCTURE;
using DOMAIN;
using DOMAIN.EnumHelper;
using DOMAIN.Paginator;
using Swashbuckle.AspNetCore.SwaggerGen;
using StructureMap.Diagnostics;

namespace API.Controllers
{
  [Route("api/[controller]")]
  public class PropostasController : Controller
  {
    
    private int PageNumber;
    private int PageSize;
    private readonly ApplicationDbContext _context;
    
    public PropostasController(ApplicationDbContext context)
    {
      _context = context;
      this.PageNumber = 1;
      this.PageSize = 20;
    }


    [HttpGet, Authorize]
    [SwaggerResponse(201,typeof(PagedList<Proposta>))]
    //[SwaggerResponse(201)]
    [SwaggerResponse(401)]
    [SwaggerResponse(403)]
    public  IActionResult Get(PaginationParamsProposta model)
    {
      var propostas = RestornaPropostaList();
      this.setPaginacao(model);
      if(!string.IsNullOrEmpty(model.NomeProposta) || !string.IsNullOrEmpty(model.Valor.ToString()) || !string.IsNullOrEmpty(model.FornecedorID)){ 
        propostas = propostas.Where(x => x.NomeProposta.Contains(model.NomeProposta) 
                            ||  x.Valor.Equals(model.Valor)
                            ||  x.Fornecedor.Id == Guid.Parse(model.FornecedorID)
                            ||  x.Categoria.Id == Guid.Parse(model.CategoriaID)
                            ).ToList();
      }
      if(propostas.Count() > 0)
        return Ok(new PagedList<Proposta>(propostas.AsQueryable(), this.PageNumber, this.PageSize));
      return Ok(new { Response = "Nenhum Resultado Encontrado" });
    }

    [Route("{id}")]
    [HttpGet, Authorize]
    [ProducesResponseType(typeof(PropostaModel), 201)]
    [SwaggerResponse(401)]
    [SwaggerResponse(403)]
    public IActionResult GetProposta(string id)
    {
      var propostas = new PropostaModel();
      if(!string.IsNullOrEmpty(id)){
        return Ok(RestornaPropostaList().FirstOrDefault(x => x.Id == Guid.Parse(id)));
      }
      return Ok(new { Response = "Nenhum Resultado Encontrado" });
    }

    [HttpPost, Authorize]
    [SwaggerResponse(201)]
    [SwaggerResponse(401)]
    [SwaggerResponse(403)]
    public async Task<IActionResult> Post([FromForm] NovaPropostaModel model)
    {
       if (model == null)
      {
          return BadRequest();
      }
      if(RestornaPropostaList().Any(x => x.NomeProposta == model.NomeProposta))
      {
        throw new ArgumentException($"O Nome da Proposta {model.NomeProposta} já esta em uso");
      }
       var proposta = new Proposta(model.NomeProposta,
                                 model.Descricao, model.Valor, 
                                ConsultaFornecedor(model.FornecedorID), 
                                ConsultaCategoria(model.CategoriaID), 
                                (PropostaStatus)Enum.ToObject(typeof(PropostaStatus),
                                 model.Status));
      if(model.Anexo != null)
      {
        using (Stream stream = vm.pdf.OpenReadStream())
        {
            using (var binaryReader = new BinaryReader(stream))
            {
                var fileContent = binaryReader.ReadBytes((int)vm.pdf.Length);
                proposta.Anexo = new PropostaAnexo(fileContent, model.Anexo.FileName, model.Anexo.ContentType);
            }
        }

      }



      _context.Propostas.Add();
      await _context.SaveChangesAsync();

      return Ok(new {Response = "Proposta salvo com sucesso"});
    }
    
    [HttpPut("{id}"), Authorize]
    [SwaggerResponse(201)]
    [SwaggerResponse(401)]
    [SwaggerResponse(403)]
    public async Task<IActionResult> Put(string id, [FromBody] NovaPropostaModel model)
    {
      if (model == null ||  string.IsNullOrEmpty(id))
      {
          return BadRequest();
      }
      
      var proposta =  ConsultaProposta(id);
      if (proposta == null)
      {
          return NotFound();
      }

      var propost = new Proposta(model.NomeProposta, model.Descricao, model.Valor, 
                                ConsultaFornecedor(model.FornecedorID), 
                                ConsultaCategoria(model.CategoriaID), 
                                (PropostaStatus)Enum.ToObject(typeof(PropostaStatus) , model.Status)
                    );
      proposta.Atualizar(propost, _context);
      _context.Propostas.Update(proposta);
      await _context.SaveChangesAsync();

      return Ok(new {Response = "Proposta atualizado com sucesso"});
    }
    
    [HttpDelete("{id}"), Authorize]
    [SwaggerResponse(201)]
    [SwaggerResponse(401)]
    [SwaggerResponse(403)]
    public IActionResult Delete(string id)
    {
      if(string.IsNullOrEmpty(id)){
          return BadRequest();
      }
      var user = ConsultaProposta(id);
      _context.Propostas.Remove(user);
      _context.SaveChanges();

      return Ok(new {Response = "Proposta deletado com sucesso"});
    }
    private List<Proposta> RestornaPropostaList(){
      return _context.Propostas.Where(x => !x.Excluido).ToList();
    }
    private Proposta ConsultaProposta(string id){
      return _context.Propostas.FirstOrDefault(x => x.Id == Guid.Parse(id) && !x.Excluido);
    }    
    private Fornecedor ConsultaFornecedor(string fornecedorID)
    {   
        return _context.Fornecedores.FirstOrDefault(x => x.Id == Guid.Parse(fornecedorID));           
    }
    private Categoria ConsultaCategoria(string categoriaID)
    {   
        return _context.Categorias.FirstOrDefault(x => x.Id == Guid.Parse(categoriaID));
    }
    private void setPaginacao(PaginationParamsProposta model)
    {
        this.PageNumber = model.PageNumber;
        this.PageSize = model.PageSize;
    }
  }
}