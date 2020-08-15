using System.Web;
using System.Web.Optimization;

namespace MarketASP
{
    public class BundleConfig
    {
        // Para obtener más información sobre las uniones, visite https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/estructura").Include(
                        "~/Scripts/jquery-ui-1.12.1.custom/external/jquery/jquery.js",
                        "~/Scripts/jquery-ui-1.12.1.custom/jquery-ui.js",
                        "~/Scripts/jquery-ui-1.12.1.custom/jquery.ui.datepicker-es.js",
                        "~/Scripts/jquery.jeditable.js",
                        "~/Scripts/DataTables/jquery.dataTables.js",
                        "~/Scripts/DataTables/dataTables.bootstrap.js",
                        "~/Scripts/DataTables/dataTables.buttons.js",
                        "~/Scripts/DataTables/buttons.flash.js",
                        "~/Scripts/DataTables/buttons.html5.js",
                        "~/Scripts/DataTables/buttons.print.js",
                        "~/Scripts/DataTables/dataTables.select.min.js"
                        ));

            // Utilice la versión de desarrollo de Modernizr para desarrollar y obtener información. De este modo, estará
            // para la producción, use la herramienta de compilación disponible en https://modernizr.com para seleccionar solo las pruebas que necesite.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new ScriptBundle("~/bundles/desarrollo").Include(
               "~/Scripts/Generica/General.js"));

            bundles.Add(new ScriptBundle("~/bundles/movimiento").Include(
               "~/Scripts/Movimientos/moviCrear.js"));

            bundles.Add(new ScriptBundle("~/bundles/venta").Include(
               "~/Scripts/Ventas/ventaCrear.js"));

            bundles.Add(new ScriptBundle("~/bundles/compra").Include(
               "~/Scripts/Compras/compraCrear.js"));


            bundles.Add(new ScriptBundle("~/bundles/cliente").Include(
               "~/Scripts/Cliente/clienteCrear.js"));


            bundles.Add(new StyleBundle("~/Content/estructura").Include(
                      "~/Content/jquery-ui-1.12.1.custom/jquery-ui.css",
                      "~/Content/jquery-ui-1.12.1.custom/jquery-ui.structure.css",
                      "~/Content/jquery-ui-1.12.1.custom/jquery-ui.theme.css",
                      "~/Content/DataTables/css/dataTables.jqueryui.css",
                      "~/Content/DataTables/css/buttons.dataTables.css"));

        }
    }
}
