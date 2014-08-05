using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy;
using Nancy.Conventions;
using Nancy.ModelBinding;

namespace Flickr2
{
    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            Get["/"] = _ => View["index.html"];
        }

    }

    public static class MemoryDb
    {
        static MemoryDb()
        {
            Ids = new List<string>();
        }

        public static IList<string> Ids { get; set; }

    }

    public class ImagesModule : NancyModule
    {
        public ImagesModule()
            : base("/images")
        {
            Post["/"] = _ =>
                {

                    var image = this.Bind<Image>();

                    MemoryDb.Ids.Add(image.Id);
                    return HttpStatusCode.OK;
                };


            Get["/"] = _ =>
                {
                    return MemoryDb.Ids;
                };
        }
    }


    public class Image
    {
        public string Id { get; set; }
    }


    public class ApplicationBootstrapper : DefaultNancyBootstrapper
    {
        protected override void ConfigureConventions(NancyConventions nancyConventions)
        {
            nancyConventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("App"));
            nancyConventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("Content"));
            nancyConventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("Scripts"));
            nancyConventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("fonts"));
            nancyConventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("Images"));
            base.ConfigureConventions(nancyConventions);
        }

        protected override void RequestStartup(Nancy.TinyIoc.TinyIoCContainer container, Nancy.Bootstrapper.IPipelines pipelines, NancyContext context)
        {
            //CORS Enable
            pipelines.AfterRequest.AddItemToEndOfPipeline((ctx) =>
            {
                ctx.Response.WithHeader("Access-Control-Allow-Origin", "*")
                                .WithHeader("Access-Control-Allow-Methods", "POST,GET")
                                .WithHeader("Access-Control-Allow-Headers", "Accept, Origin, Content-type");

            });
        }

    }
}