using System.Data;
using System.Web;
using System.Web.Optimization;

namespace MarketASP
{
    public class BundleConfig
    {
        // Para obtener más información sobre las uniones, visite https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //bundles.UseCdn = true;

            #region JS

                bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                            "~/Scripts/jquery-{version}.js"));

                bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                            "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new Bundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.bundle.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/template").Include(
                    "~/Scripts/assets/js/app.js"));



            bundles.Add(new ScriptBundle("~/bundles/assets").Include(
                    "~/Scripts/assets/libs/metismenu/metisMenu.min.js",
                    "~/Scripts/assets/libs/simplebar/simplebar.min.js",
                    "~/Scripts/assets/libs/node-waves/waves.min.js",
                    "~/Scripts/assets/js/pages/datatables.init.js",
                    "~/Scripts/assets/js/app.js"));

            bundles.Add(new ScriptBundle("~/bundles/tablas").Include(
                            "~/Scripts/librerias/jquery-ui-1.12.1.custom/external/jquery/jquery.js",
                            "~/Scripts/librerias/jquery-ui-1.12.1.custom/jquery-ui.js",
                            "~/Scripts/librerias/jquery-ui-1.12.1.custom/jquery.ui.datepicker-es.js",
                            "~/Scripts/jquery.jeditable.js",
                            "~/Scripts/DataTables/jquery.dataTables.js",
                            "~/Scripts/DataTables/dataTables.bootstrap4.js",
                            "~/Scripts/DataTables/dataTables.buttons.js",
                            "~/Scripts/DataTables/buttons.flash.js",
                            "~/Scripts/DataTables/buttons.html5.js",
                            "~/Scripts/DataTables/buttons.print.js",
                            "~/Scripts/DataTables/dataTables.select.min.js",
                            "~/Scripts/Datatables/dataTables.responsive.min.js",
                            "~/Scripts/Datatables/responsive.bootstrap4.min.js"
                            ));



            //bundles.Add(new ScriptBundle("~/bundles/estructura").Include(
            //                "~/Scripts/jquery-ui-1.12.1.custom/external/jquery/jquery.js",
            //                "~/Scripts/jquery-ui-1.12.1.custom/jquery-ui.js",
            //                "~/Scripts/jquery-ui-1.12.1.custom/jquery.ui.datepicker-es.js",
            //                "~/Scripts/jquery.jeditable.js",
            //                "~/Scripts/DataTables/jquery.dataTables.js",
            //                "~/Scripts/DataTables/dataTables.bootstrap4.js",
            //                "~/Scripts/DataTables/dataTables.buttons.js",
            //                "~/Scripts/DataTables/buttons.flash.js",
            //                "~/Scripts/DataTables/buttons.html5.js",
            //                "~/Scripts/DataTables/buttons.print.js",
            //                "~/Scripts/DataTables/dataTables.select.min.js"
            //                ));



            #endregion

            #region CSS
            var jquerycdnpath = "https://fonts.googleapis.com/css?family=Nunito:200,200i,300,300i,400,400i,600,600i,700,700i,800,800i,900,900i";
            bundles.Add(new StyleBundle("~/fonts", jquerycdnpath));


            bundles.Add(new StyleBundle("~/Content/css/css").Include(
                  "~/Content/css/bootstrap.min.css",
                  "~/Content/css/icons.min.css",
                  "~/Content/css/app.min.css"));

            bundles.Add(new StyleBundle("~/Content/librerias/css").Include(
                      "~/Content/jquery-ui-1.12.1.custom/jquery-ui.css",
                      "~/Content/jquery-ui-1.12.1.custom/jquery-ui.structure.css",
                      "~/Content/jquery-ui-1.12.1.custom/jquery-ui.theme.css"));


            bundles.Add(new StyleBundle("~/Content/DataTables/css/css").Include(
                      "~/Content/DataTables/css/dataTables.bootstrap4.css",
                      "~/Content/DataTables/css/buttons.bootstrap4.min.css",
                      "~/Content/DataTables/css/select.bootstrap4.min.css",
                      "~/Content/DataTables/css/responsive.bootstrap4.min.css",
                      "~/Content/DataTables/css/dataTables.jqueryui.css"));




            #endregion

            #region Personalizado

                bundles.Add(new ScriptBundle("~/bundles/desarrollo").Include(
                   "~/Scripts/Generica/General.js"));

                bundles.Add(new ScriptBundle("~/bundles/movimiento").Include(
                   "~/Scripts/Movimientos/moviCrear.js"));

            bundles.Add(new ScriptBundle("~/bundles/proforma").Include(
               "~/Scripts/proforma/profoCrear.js"));

            bundles.Add(new ScriptBundle("~/bundles/ordenpedido").Include(
               "~/Scripts/ordenpedido/orpedidoCrear.js"));


            bundles.Add(new ScriptBundle("~/bundles/venta").Include(
                   "~/Scripts/Ventas/ventaCrear.js"));

            bundles.Add(new ScriptBundle("~/bundles/ordencompra").Include(
               "~/Scripts/ordencompra/orcompraCrear.js"));

            bundles.Add(new ScriptBundle("~/bundles/compra").Include(
                   "~/Scripts/Compras/compraCrear.js"));

            bundles.Add(new ScriptBundle("~/bundles/compraLote").Include(
                   "~/Scripts/Compras/compraCrearLote.js"));

            bundles.Add(new ScriptBundle("~/bundles/cliente").Include(
                   "~/Scripts/Cliente/clienteCrear.js"));

                bundles.Add(new ScriptBundle("~/bundles/ctacobrar").Include(
                   "~/Scripts/CtaCobrar/ctacobrarCrea.js"));

            bundles.Add(new ScriptBundle("~/bundles/guia").Include(
               "~/Scripts/Guia/guiagral.js"));

            #endregion



        }
    }
}
