namespace Souccar.NHibernate.FluentNHibernate
{
    using System.Xml;

    /// <summary>
    ///     Facilitates the visitor pattern for <see cref = "GeneratorHelper" /> to spit out the NHibernate
    ///     XML for the class.
    /// 
    ///     To use, have your mapper implement this interface.  Then, simply include the following line within
    ///     Generate():  return CreateMapping(new MappingVisitor());
    /// 
    ///     Now you can call Generate on your mapper class to view the generated XML.
    /// </summary>
    /// <remarks>
    ///     This is not necessary for Fluent Nhibernate to function properly.
    /// </remarks>
    public interface IMapGenerator
    {
        string FileName { get; }

        XmlDocument Generate();
    }
}