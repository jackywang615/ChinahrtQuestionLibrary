using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ChinahrtQuestionLibrary.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChinahrtQuestionLibrary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        private ChinahrtContext context;

        public QuestionsController(ChinahrtContext context)
        {
            this.context = context;
        }

        // GET api/values/5
        [HttpGet]
        public ActionResult<IEnumerable<Question>> Get([FromQuery]string title)
        {
            Regex titleReg = new Regex(@"(?:\d+\.)(?<title>.+)(?:（\d+\.\d分）)");
            var m_title = titleReg.Match(title);
            var t = m_title.Groups["title"].Value;

            var results = context.Questions.Where(x => x.Title == t);
            foreach (Question q in results)
            {
                if (q.Answers.Any(x => x.IsRight))
                {
                    q.Answers.RemoveAll(x => x.IsRight == false);
                }
            }
            return results.ToList();
        }

        // POST api/values
        [HttpPost]
        public void Post([FromForm]PaperResult[] data)
        {
            Regex titleReg = new Regex(@"(?:\d+\.)(?<title>.*)(?:（\d+\.\d分）)");
            Regex valueReg = new Regex("(?:我的答案：)(?<result>.*)(?<isRight>(?:√答对)|(?:×答错))");

            foreach (var paperResult in data)
            {
                var m_title = titleReg.Match(paperResult.Title);
                var m_value = valueReg.Match(paperResult.Value);
                var title = m_title.Groups["title"].Value;

                var results = context.Questions.Where(x => x.Title == title).ToList();
                if (results?.Count > 0)
                {
                    foreach (var i in results)
                    {
                        if (i.Answers.All(x => x.Result != m_value.Groups["result"].Value))
                        {
                            i.Answers.Add(new Answer() { Result = m_value.Groups["result"].Value, IsRight = m_value.Groups["isRight"].Value.Contains("答对") });
                        }

                        context.Questions.Update(i);
                    }
                }
                else
                {
                    var q = new Question();
                    q.Title = title;
                    q.Answers.Add(new Answer() { Result = m_value.Groups["result"].Value, IsRight = m_value.Groups["isRight"].Value.Contains("答对") });
                    context.Questions.Add(q);
                }
            }
            context.SaveChanges();
        }
    }
}