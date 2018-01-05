using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using InformationServer.Models;
using System.Data.Entity.Infrastructure;

namespace InformationServer.Controllers
{
    [RoutePrefix("conference/{section}/info")]
    public class ConferenceController : ApiController
    {
        [HttpGet]
        [Route("")]
        public IHttpActionResult Get(string section)
        {
            using (ServicesEntities db = new ServicesEntities())
            {
                var Section = db.Sections.Where(s => s.Section == section).FirstOrDefault();
                if (Section != null)
                    return Ok(Section);

            }
            return NotFound();
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult Post([FromUri]string section, [FromBody]Sections SectionEntity)
        {
            using (ServicesEntities db = new ServicesEntities())
            {
                try
                {
                    if (db.Sections.Any(s => s.Section == section))
                        return BadRequest("Запись с данным идентификатором сервера уже создана.");
                    SectionEntity.Id = Guid.NewGuid();
                    SectionEntity.Section = section;
                    db.Sections.Add(SectionEntity);
                    db.SaveChanges();
                    return Ok();
                }
                catch(Exception ex)
                {
                    return BadRequest(ex.ToString());
                }
            }
        }

        [HttpPut]
        [Route("")]
        public IHttpActionResult Put([FromUri]string section, [FromBody]Sections SectionEntity)
        {
            using (ServicesEntities db = new ServicesEntities())
            {
                try
                {
                    var UpdatedEntity = db.Sections.Where(s => s.Section == section).FirstOrDefault();
                    if (UpdatedEntity != null)
                    {
                        UpdatedEntity.City = SectionEntity.City;
                        UpdatedEntity.Location = SectionEntity.Location;
                        UpdatedEntity.Name = SectionEntity.Name;
                        db.Entry(UpdatedEntity).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        return Ok();
                    }
                    else
                        return BadRequest("Записи с данным идентификатором сервера не найдено.");
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.ToString());
                }
            }
        }

        [HttpGet]
        [Route("~/conference/info")]
        public IHttpActionResult Get()
        {
            using (ServicesEntities db = new ServicesEntities())
            {
                return Ok(db.Sections.ToList().Select(section => new {
                                                                        section = section.Section,
                                                                        info = new {
                                                                            name = section.Name,
                                                                            city = section.City,
                                                                            location = section.Location
                                                                        }
                                                                     }).OrderBy(s => s.section));
            }
        }
    }
}
