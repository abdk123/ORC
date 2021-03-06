using System;
using System.Collections.Generic;
using System.Web.Mvc;
using UI.Extensions;
namespace UI.Filters
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public class SourceCodeFileAttribute : FilterAttribute, IResultFilter
    {
        private readonly string caption;
        private readonly string filename;

        public SourceCodeFileAttribute(string caption, string filename)
        {
            this.caption = caption;
            this.filename = filename;
        }

        public string Caption
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string FileName
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string RazorFileName
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public void OnResultExecuting(ResultExecutingContext filterContext)
        {
            ViewDataDictionary viewData = filterContext.Controller.ViewData;

            var codeFiles = viewData.Get<Dictionary<string, string>>("codeFiles");     
            codeFiles[caption] = filename;
        }

        public void OnResultExecuted(ResultExecutedContext filterContext)
        {
            // Do Nothing
        }
    }
}