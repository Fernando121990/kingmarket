//servicio sunat
using ServiceSunat;
//referencia de proyecto
using SunatService.Modelos;
using SunatService.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
//using ServiceGUIA;

namespace SunatService.Servicios
{
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ProgId("firmadoCE.firmado")]
    public  class SUNAT_UTIL
    {

        public string RUCEmpresa { get; set; }
        public  string RazonSocialEmpresa { get; set; }
        public  string Ruta_XML { get; set; }
        public  string Ruta_Certificado { get; set; }
        public  string Password_Certificado { get; set; }
        public  string Ruta_ENVIOS { get; set; }
        public  string Ruta_CDRS { get; set; }

        public SunatResponse GenerarComprobanteFB_XML(Cabecera Comprobante)
        {
           
            SunatService.Modelos.InvoiceType Factura = new SunatService.Modelos.InvoiceType();

            try
            {
                //------Namespace necesarios para el UBL
                Factura.Cac = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2";
                Factura.Cbc = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2";
                Factura.Ccts = "urn:un:unece:uncefact:documentation:2";
                Factura.Ds = "http://www.w3.org/2000/09/xmldsig#";
                Factura.Ext = "urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2";
                Factura.Qdt = "urn:oasis:names:specification:ubl:schema:xsd:QualifiedDatatypes-2";
                Factura.Udt = "urn:un:unece:uncefact:data:specification:UnqualifiedDataTypesSchemaModule:2";
                //------                
                SunatService.Modelos.UBLExtensionType[] ublExtensiones = new SunatService.Modelos.UBLExtensionType[5];
                SunatService.Modelos.UBLExtensionType ublExtension = new SunatService.Modelos.UBLExtensionType();

                ublExtensiones[0] = ublExtension;
                Factura.UBLExtensions = ublExtensiones;

                Factura.UBLVersionID = new SunatService.Modelos.UBLVersionIDType();
                Factura.UBLVersionID.Value = "2.1";

                Factura.CustomizationID = new SunatService.Modelos.CustomizationIDType();
                //Factura.CustomizationID.schemeAgencyName = "PE:SUNAT";
                Factura.CustomizationID.Value = "2.0";
                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                //Código de tipo de operación                
                Factura.ID = new SunatService.Modelos.IDType();
                Factura.ID.Value = Comprobante.Serie + "-" + Comprobante.Numero;
                //Fecha de emisión y hora de emision
                Factura.IssueDate = new SunatService.Modelos.IssueDateType();
                string fechaemision = Convert.ToDateTime(Comprobante.Fechaemision).ToString("dd/MM/yyyy");
                Factura.IssueDate.Value = Convert.ToDateTime(fechaemision);
                Factura.IssueTime = new SunatService.Modelos.IssueTimeType();
                //   string hora = Convert.ToDateTime(Comprobante.Cab_Fac_Fecha).ToString("HH:mm:ss");
                Factura.IssueTime.Value = DateTime.Now.ToString("HH:mm:ss");
                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                //Fecha de Vencimiento 
                Factura.DueDate = new SunatService.Modelos.DueDateType();
                Factura.DueDate.Value = Convert.ToDateTime(Comprobante.Fechavencimiento);
                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                //Tipo de documento(Factura)
                SunatService.Modelos.InvoiceTypeCodeType TipoDoc = new SunatService.Modelos.InvoiceTypeCodeType();
                TipoDoc.Value = Comprobante.Idtipocomp.ToString();
                TipoDoc.listAgencyName = "PE:SUNAT";
                TipoDoc.listName = "SUNAT:Identificador de Tipo de Documento";
                TipoDoc.listURI = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo01";
                //TipoDoc.name = "Tipo de Operacion";
                TipoDoc.listID = "0101";
                Factura.InvoiceTypeCode = TipoDoc;

                //Leyenda del comprobante
                SunatService.Modelos.NoteType Leyenda = new SunatService.Modelos.NoteType();
                Leyenda.languageLocaleID = "1000";
                Leyenda.Value = "MONTO EN SOLES";
                List<SunatService.Modelos.NoteType> notasLeyenda = new List<Modelos.NoteType>();
                notasLeyenda.Add(Leyenda);
                Factura.Note = notasLeyenda.ToArray();

                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                //Tipo de Moneda
                SunatService.Modelos.DocumentCurrencyCodeType moneda = new SunatService.Modelos.DocumentCurrencyCodeType()
                {
                    //listSchemeURI = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo51",
                    listID = "ISO 4217 Alpha",
                    listName = "Currency",
                    listAgencyName = "United Nations Economic Commission for Europe",
                    Value = Comprobante.Idmoneda
                };

                Factura.DocumentCurrencyCode = moneda;
                Modelos.LineCountNumericType numitems = new Modelos.LineCountNumericType();
                numitems.Value = Comprobante.Detalles.Count;
                Factura.LineCountNumeric = numitems;

                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////            
                //Nombre Comercial del emisor Apellidos y nombres, denominación o razón social del emisor Tipo y Número de RUC del emisor Código del domicilio fiscal 
                //o de local anexo del emisor 
                SunatService.Modelos.SignatureType Firma = new SunatService.Modelos.SignatureType();
                SunatService.Modelos.SignatureType[] Firmas = new SunatService.Modelos.SignatureType[2];

                SunatService.Modelos.PartyType partySign = new SunatService.Modelos.PartyType();
                SunatService.Modelos.PartyIdentificationType partyIdentificacion = new SunatService.Modelos.PartyIdentificationType();
                SunatService.Modelos.PartyIdentificationType[] partyIdentificacions = new SunatService.Modelos.PartyIdentificationType[2];
                SunatService.Modelos.IDType idFirma = new SunatService.Modelos.IDType();
                idFirma.Value = Comprobante.EmpresaRUC;
                Firma.ID = idFirma;

                partyIdentificacion.ID = idFirma;
                partyIdentificacions[0] = partyIdentificacion;
                partySign.PartyIdentification = partyIdentificacions;
                Firma.SignatoryParty = partySign;

                SunatService.Modelos.NoteType Nota = new SunatService.Modelos.NoteType();
                SunatService.Modelos.NoteType[] Notas = new SunatService.Modelos.NoteType[2];
                Nota.Value = "Elaborado por Sistema de Emision Electronica NET SOLUTION DEVELOPER ";
                Notas[0] = Nota;
                Firma.Note = Notas;

                SunatService.Modelos.PartyNameType partyName = new SunatService.Modelos.PartyNameType();
                SunatService.Modelos.PartyNameType[] partyNames = new SunatService.Modelos.PartyNameType[2];

                SunatService.Modelos.NameType1 RazonSocialFirma = new SunatService.Modelos.NameType1();
                RazonSocialFirma.Value = Comprobante.EmpresaRazonSocial;
                partyName.Name = RazonSocialFirma;
                partyNames[0] = partyName;
                partySign.PartyName = partyNames;

                SunatService.Modelos.AttachmentType attachType = new SunatService.Modelos.AttachmentType();
                SunatService.Modelos.ExternalReferenceType externaReferencia = new SunatService.Modelos.ExternalReferenceType();
                SunatService.Modelos.URIType uri = new SunatService.Modelos.URIType();
                uri.Value = "SIGN";
                externaReferencia.URI = uri;
                Firma.DigitalSignatureAttachment = attachType;

                attachType.ExternalReference = externaReferencia;
                Firma.DigitalSignatureAttachment = attachType;

                Firmas[0] = Firma;
                Factura.Signature = Firmas;

                SunatService.Modelos.SupplierPartyType empresa = new SunatService.Modelos.SupplierPartyType();
                SunatService.Modelos.PartyType party = new SunatService.Modelos.PartyType();
                SunatService.Modelos.PartyIdentificationType partyidentificacion = new SunatService.Modelos.PartyIdentificationType();
                SunatService.Modelos.PartyIdentificationType[] partyidentificacions = new SunatService.Modelos.PartyIdentificationType[2];
                SunatService.Modelos.IDType idEmpresa = new SunatService.Modelos.IDType();
                idEmpresa.schemeURI = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo06";
                idEmpresa.schemeName = "Documento de Identidad";
                idEmpresa.schemeID = "6";
                idEmpresa.schemeAgencyName = "PE:SUNAT";
                idEmpresa.Value = Comprobante.EmpresaRUC;

                partyidentificacion.ID = idEmpresa;
                partyidentificacions[0] = partyidentificacion;
                party.PartyIdentification = partyidentificacions;

                SunatService.Modelos.PartyNameType partyname = new SunatService.Modelos.PartyNameType();
                List<SunatService.Modelos.PartyNameType> partynames = new List<SunatService.Modelos.PartyNameType>();
                SunatService.Modelos.NameType1 nameEmisor = new SunatService.Modelos.NameType1();
                nameEmisor.Value = Comprobante.EmpresaRazonSocial;
                partyname.Name = nameEmisor;
                partynames.Add(partyname);
                party.PartyName = partynames.ToArray();

                SunatService.Modelos.PartyTaxSchemeType PartyTaxScheme = new SunatService.Modelos.PartyTaxSchemeType();
                List<SunatService.Modelos.PartyTaxSchemeType> PartyTaxSchemes = new List<SunatService.Modelos.PartyTaxSchemeType>();

                SunatService.Modelos.RegistrationNameType registerNameEmisor = new SunatService.Modelos.RegistrationNameType();
                registerNameEmisor.Value = Comprobante.EmpresaRazonSocial;
                PartyTaxScheme.RegistrationName = registerNameEmisor;
                //Direccion emisor                
                Modelos.CompanyIDType compañia = new Modelos.CompanyIDType();
                compañia.schemeURI = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo06";
                compañia.schemeAgencyName = "PE:SUNAT";
                compañia.schemeName = "SUNAT:Identificador de Documento de Identidad";
                compañia.schemeID = "6";
                compañia.Value = Comprobante.EmpresaRUC;


                SunatService.Modelos.AddressType direccion = new SunatService.Modelos.AddressType();
                SunatService.Modelos.AddressTypeCodeType addrestypecode = new SunatService.Modelos.AddressTypeCodeType();
                //addrestypecode.listName = "Establecimientos anexos"; 
                //addrestypecode.listAgencyName="PE:SUNAT";
                addrestypecode.Value = "0000";
                direccion.AddressTypeCode = addrestypecode;
                PartyTaxScheme.RegistrationAddress = direccion;
                //Modelos.IDType tipo = new Modelos.IDType();
                Modelos.TaxSchemeType taxSchema = new Modelos.TaxSchemeType();
                Modelos.IDType idsupplier = new Modelos.IDType();
                idsupplier.schemeURI = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo06";
                idsupplier.schemeAgencyName = "PE:SUNAT";
                idsupplier.schemeName = "SUNAT:Identificador de Documento de Identidad";
                idsupplier.schemeID = "6";
                idsupplier.Value = Comprobante.EmpresaRUC;
                taxSchema.ID = idsupplier;

                PartyTaxScheme.CompanyID = compañia;
                PartyTaxScheme.TaxScheme = taxSchema;
                PartyTaxSchemes.Add(PartyTaxScheme);
                //party.PartyTaxScheme = PartyTaxSchemes.ToArray();

                List<Modelos.PartyLegalEntityType> partelegals = new List<Modelos.PartyLegalEntityType>();
                Modelos.PartyLegalEntityType partelegal = new Modelos.PartyLegalEntityType();
                SunatService.Modelos.RegistrationNameType registerNamePL = new SunatService.Modelos.RegistrationNameType();
                registerNamePL.Value = Comprobante.EmpresaRazonSocial;
                partelegal.RegistrationName = registerNamePL;

                Modelos.AddressType direccionPL = new Modelos.AddressType();
                Modelos.IDType iddireccionPL = new Modelos.IDType();
                //iddireccionPL.schemeAgencyName = "PE:INEI";
                //iddireccionPL.schemeName = "Ubigeos";
                iddireccionPL.Value = Comprobante.ID_EmpresaDepartamento + Comprobante.ID_EmpresaProvincia + Comprobante.ID_EmpresaDistrito;
                direccionPL.ID = iddireccionPL;

                Modelos.AddressTypeCodeType address_TypeCodeType = new Modelos.AddressTypeCodeType();
                address_TypeCodeType.listName = "Establecimientos anexos";
                address_TypeCodeType.listAgencyName = "PE:SUNAT";
                address_TypeCodeType.Value = "0000";
                direccionPL.AddressTypeCode = address_TypeCodeType;

                SunatService.Modelos.CityNameType Departamento = new SunatService.Modelos.CityNameType();
                Departamento.Value = Comprobante.EmpresaDepartamento;
                direccionPL.CityName = Departamento;

                SunatService.Modelos.CountrySubentityType Provincia = new SunatService.Modelos.CountrySubentityType();
                Provincia.Value = Comprobante.EmpresaProvincia;
                direccionPL.CountrySubentity = Provincia;

                SunatService.Modelos.DistrictType distrito = new SunatService.Modelos.DistrictType();
                distrito.Value = Comprobante.EmpresaDistrito;
                direccionPL.District = distrito;
                List<Modelos.AddressLineType> direcciones = new List<Modelos.AddressLineType>();
                Modelos.AddressLineType direccionEmisor = new Modelos.AddressLineType();
                Modelos.LineType local1 = new Modelos.LineType();
                local1.Value = Comprobante.EmpresaDireccion;
                direccionPL.AddressLine = direcciones.ToArray();
                direccionEmisor.Line = local1;
                direcciones.Add(direccionEmisor);
                direccionPL.AddressLine = direcciones.ToArray();

                SunatService.Modelos.CountryType pais = new SunatService.Modelos.CountryType();
                SunatService.Modelos.IdentificationCodeType codigoPais = new SunatService.Modelos.IdentificationCodeType();

                codigoPais.listName = "Country";
                codigoPais.listAgencyName = "United Nations Economic Commission for Europe";
                codigoPais.listID = "ISO 3166-1";
                codigoPais.Value = "PE";
                pais.IdentificationCode = codigoPais;

                direccionPL.Country = pais;
                partelegal.RegistrationAddress = direccionPL;

                partelegals.Add(partelegal);
                party.PartyLegalEntity = partelegals.ToArray();

                // partySchema.TaxScheme = taxSchema;
                //tipo.schemeAgencyName = "PE:INEI";
                //tipo.schemeName = "Ubigeos";
                //tipo.Value = Comprobante.ID_EmpresaDepartamento + Comprobante.ID_EmpresaProvincia + Comprobante.ID_EmpresaDistrito; //Ubigeo Emisor                ;                

                //SunatService.Modelos.CitySubdivisionNamdeType Subdivisionname = new SunatService.Modelos.CitySubdivisionNameType();
                //Subdivisionname.Value = Comprobante.EmpresaDireccion;



                //SunatService.Modelos.CountrySubentityCodeType codigo = new SunatService.Modelos.CountrySubentityCodeType();
                //codigo.Value = "01";

                //SunatService.Modelos.AddressLineType calle = new SunatService.Modelos.AddressLineType();
                //SunatService.Modelos.AddressLineType[] calles = new SunatService.Modelos.AddressLineType[2];
                //calles[0] = calle;
                //SunatService.Modelos.LineType nombreCalle = new SunatService.Modelos.LineType();
                //nombreCalle.Value = Comprobante.EmpresaDireccion;
                //calle.Line = nombreCalle;

                // direccion.AddressTypeCode = addrestypecode;
                // //direccion.CitySubdivisionName = Subdivisionname;
                // direccion.CityName = Provincia;
                // direccion.CountrySubentity = Departamento;
                // //direccion.CountrySubentityCode = codigo;
                // direccion.District = distrito;
                // direccion.AddressLine = direcciones.ToArray();
                // direccion.Country = pais;


                // PartyTaxScheme.RegistrationAddress = direccion;
                // PartyTaxSchemes.Add(PartyTaxScheme);                
                //party.PartyTaxScheme = PartyTaxSchemes.ToArray();

                party.PartyName = partynames.ToArray();
                party.PartyIdentification = partyidentificacions;
                empresa.Party = party;
                Factura.AccountingSupplierParty = empresa;

                //EMPRESA CLIENTE
                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////            
                //Tipo y número de documento de identidad del adquirente o usuario Apellidos y nombres, denominación o razón social del adquirente o usuario
                SunatService.Modelos.TaxSchemeType taxschemeCliente = new SunatService.Modelos.TaxSchemeType();
                SunatService.Modelos.CustomerPartyType CustomerPartyCliente = new SunatService.Modelos.CustomerPartyType();
                SunatService.Modelos.PartyType partyCliente = new SunatService.Modelos.PartyType();
                SunatService.Modelos.PartyIdentificationType partyIdentificion = new SunatService.Modelos.PartyIdentificationType();
                List<SunatService.Modelos.PartyIdentificationType> partyIdentificions = new List<SunatService.Modelos.PartyIdentificationType>();
                SunatService.Modelos.IDType idtipo = new SunatService.Modelos.IDType();
                idtipo.schemeURI = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo06";
                idtipo.schemeName = "Documento de Identidad";
                idtipo.schemeAgencyName = "PE:SUNAT";
                idtipo.schemeID = Comprobante.ClienteTipodocumento;
                idtipo.Value = Comprobante.ClienteNumeroDocumento;
                partyIdentificion.ID = idtipo;

                partyIdentificions.Add(partyIdentificion);
                partyCliente.PartyIdentification = partyIdentificions.ToArray();

                List<SunatService.Modelos.PartyNameType> RazSocClientes = new List<SunatService.Modelos.PartyNameType>();
                SunatService.Modelos.PartyNameType RazSocCliente = new SunatService.Modelos.PartyNameType();
                Modelos.NameType1 razSocial = new Modelos.NameType1();
                razSocial.Value = Comprobante.ClienteRazonSocial;
                RazSocCliente.Name = razSocial;
                RazSocClientes.Add(RazSocCliente);
                //partyCliente.PartyName = RazSocClientes.ToArray();


                List<SunatService.Modelos.PartyTaxSchemeType> partySchemas = new List<Modelos.PartyTaxSchemeType>();
                SunatService.Modelos.PartyTaxSchemeType partySchema = new SunatService.Modelos.PartyTaxSchemeType();
                Modelos.RegistrationNameType RegistroNombre = new Modelos.RegistrationNameType();
                RegistroNombre.Value = Comprobante.ClienteRazonSocial;
                partySchema.RegistrationName = RegistroNombre;

                Modelos.CompanyIDType idcompañia = new Modelos.CompanyIDType();
                idcompañia.schemeURI = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo06";
                idcompañia.schemeAgencyName = "PE:SUNAT";
                idcompañia.schemeName = "SUNAT:Identificador de Documento de Identidad";
                idcompañia.schemeID = Comprobante.ClienteTipodocumento;
                idcompañia.Value = Comprobante.ClienteNumeroDocumento;

                Modelos.TaxSchemeType schemeType = new Modelos.TaxSchemeType();
                Modelos.IDType idc = new Modelos.IDType();
                idc.schemeURI = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo06";
                idc.schemeAgencyName = "PE:SUNAT";
                idc.schemeName = "SUNAT:Identificador de Documento de Identidad";
                idc.schemeID = Comprobante.ClienteTipodocumento;
                idc.Value = Comprobante.ClienteNumeroDocumento;
                schemeType.ID = idc;

                idcompañia.schemeURI = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo06";
                idcompañia.schemeAgencyName = "PE:SUNAT";
                idcompañia.schemeName = "SUNAT:Identificador de Documento de Identidad";
                idcompañia.schemeID = Comprobante.ClienteTipodocumento;
                idcompañia.Value = Comprobante.ClienteNumeroDocumento;

                List<Modelos.PartyLegalEntityType> partyLegals = new List<Modelos.PartyLegalEntityType>();
                Modelos.PartyLegalEntityType partyLegal = new Modelos.PartyLegalEntityType();
                Modelos.RegistrationNameType Registro_Nombre = new Modelos.RegistrationNameType();
                Registro_Nombre.Value = Comprobante.ClienteRazonSocial;
                partyLegal.RegistrationName = Registro_Nombre;
                Modelos.AddressType direccionCliente = new Modelos.AddressType();
                List<Modelos.AddressLineType> dirs = new List<Modelos.AddressLineType>();
                Modelos.AddressLineType dir = new Modelos.AddressLineType();
                List<Modelos.LineType> lineas = new List<Modelos.LineType>();
                Modelos.LineType linea = new Modelos.LineType();
                linea.Value = Comprobante.ClienteDireccion;
                dir.Line = linea;
                dirs.Add(dir);
                direccionCliente.AddressLine = dirs.ToArray();
                partyLegal.RegistrationAddress = direccionCliente;

                SunatService.Modelos.CountryType paisC = new SunatService.Modelos.CountryType();
                SunatService.Modelos.IdentificationCodeType codigoPaisC = new SunatService.Modelos.IdentificationCodeType();

                //codigoPaisC.listName = "Country";
                //codigoPaisC.listAgencyName = "United Nations Economic Commission for Europe";
                //codigoPaisC.listID = "ISO 3166-1";
                codigoPaisC.Value = "PE";
                paisC.IdentificationCode = codigoPaisC;
                partyLegal.RegistrationAddress.Country = paisC;
                partyLegals.Add(partyLegal);


                partySchema.CompanyID = idcompañia;
                partySchema.TaxScheme = schemeType;


                partySchemas.Add(partySchema);
                //partyCliente.PartyTaxScheme = partySchemas.ToArray();
                partyCliente.PartyLegalEntity = partyLegals.ToArray();

                CustomerPartyCliente.Party = partyCliente;
                Modelos.CustomerPartyType accoutingCustomerParty = new Modelos.CustomerPartyType();
                accoutingCustomerParty.Party = partyCliente;
                ///CustomerPartyCliente = partyCliente;
                Factura.AccountingCustomerParty = accoutingCustomerParty;

                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                //Monto total de impuestos
                //Monto las operaciones gravadas
                //Monto las operaciones Exoneradas
                //Monto las operaciones inafectas del impuesto(Ver Ejemplo en la página 47)
                //Monto las operaciones gratuitas(Ver Ejemplo en la página 48)
                //Sumatoria de IGV
                //Sumatoria de ISC(Ver Ejemplo en la página 51)
                //Sumatoria de Otros Tributos(Ver Ejemplo en la página 52)

                SunatService.Modelos.TaxTotalType TotalImptos = new SunatService.Modelos.TaxTotalType();
                SunatService.Modelos.TaxAmountType taxAmountImpto = new SunatService.Modelos.TaxAmountType();
                taxAmountImpto.currencyID = Comprobante.Idmoneda;
                taxAmountImpto.Value = Convert.ToDecimal(Comprobante.TotIgv);
                TotalImptos.TaxAmount = taxAmountImpto;
                //////////////////////////////////////////////////////////////////////////////
                ///
                SunatService.Modelos.TaxSubtotalType[] subtotales = new SunatService.Modelos.TaxSubtotalType[2];
                SunatService.Modelos.TaxSubtotalType subtotal = new SunatService.Modelos.TaxSubtotalType();

                SunatService.Modelos.TaxableAmountType taxsubtotal = new SunatService.Modelos.TaxableAmountType();
                taxsubtotal.currencyID = Comprobante.Idmoneda;
                taxsubtotal.Value = Convert.ToDecimal(Comprobante.TotSubtotal);
                subtotal.TaxableAmount = taxsubtotal;

                SunatService.Modelos.TaxAmountType TotalTaxAmountTotal = new SunatService.Modelos.TaxAmountType();
                TotalTaxAmountTotal.currencyID = Comprobante.Idmoneda;
                TotalTaxAmountTotal.Value = Convert.ToDecimal(Comprobante.TotIgv);
                subtotal.TaxAmount = TotalTaxAmountTotal;

                Modelos.TaxSubtotalType subTotalIGV = new Modelos.TaxSubtotalType();
                subTotalIGV.TaxableAmount = taxsubtotal;

                subtotales[0] = subtotal;
                TotalImptos.TaxSubtotal = subtotales;


                //PAgo de IGV
                SunatService.Modelos.TaxCategoryType taxcategoryTotal = new SunatService.Modelos.TaxCategoryType();
                SunatService.Modelos.TaxSchemeType taxScheme = new SunatService.Modelos.TaxSchemeType();
                SunatService.Modelos.IDType idTotal = new SunatService.Modelos.IDType();
                idTotal.schemeID = "UN/ECE 5305";
                idTotal.schemeName = "Tax Category Identifier";
                idTotal.schemeAgencyName = "United Nations Economic Commission for Europe";
                idTotal.Value = "S";
                //taxcategoryTotal.ID = idTotal;
                SunatService.Modelos.NameType1 nametypeImpto = new SunatService.Modelos.NameType1();
                nametypeImpto.Value = "IGV";
                SunatService.Modelos.TaxTypeCodeType taxtypecodeImpto = new SunatService.Modelos.TaxTypeCodeType();
                taxtypecodeImpto.Value = "VAT";

                SunatService.Modelos.IDType idTot = new SunatService.Modelos.IDType();
                //idTot.schemeID = "UN/ECE 5153";
                //idTot.schemeAgencyID = "6";
                idTot.Value = "1000";
                taxScheme.ID = idTot;

                SunatService.Modelos.NameType1 nametypeImptoIGV = new SunatService.Modelos.NameType1();
                nametypeImptoIGV.Value = "IGV";
                SunatService.Modelos.TaxTypeCodeType taxtypecodeImpuesto = new SunatService.Modelos.TaxTypeCodeType();
                taxtypecodeImpuesto.Value = "VAT";

                taxScheme.Name = nametypeImpto;
                taxScheme.TaxTypeCode = taxtypecodeImpto;
                taxcategoryTotal.TaxScheme = taxScheme;
                subtotal.TaxCategory = taxcategoryTotal;

                SunatService.Modelos.TaxSubtotalType[] TaxSubtotals = new SunatService.Modelos.TaxSubtotalType[2];
                TaxSubtotals[0] = subtotal;
                TotalImptos.TaxSubtotal = TaxSubtotals;
                SunatService.Modelos.TaxTotalType[] taxTotals = new SunatService.Modelos.TaxTotalType[2];
                taxTotals[0] = TotalImptos;
                Factura.TaxTotal = taxTotals;
                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////                                  
                ////Total valor de venta 
                ////Total precio de venta(incluye impuestos) 
                ////Monto total de descuentos del comprobante 
                ////Monto total de otros cargos del comprobante 
                ////Importe total de la venta, cesión en uso o del servicio prestado
                SunatService.Modelos.MonetaryTotalType TotalValorVenta = new SunatService.Modelos.MonetaryTotalType();
                SunatService.Modelos.LineExtensionAmountType TValorVenta = new SunatService.Modelos.LineExtensionAmountType();

                TValorVenta.currencyID = Comprobante.Idmoneda;
                TValorVenta.Value = Convert.ToDecimal(string.Format("{0:0.00}", Comprobante.TotSubtotal));
                TotalValorVenta.LineExtensionAmount = TValorVenta;

                SunatService.Modelos.TaxInclusiveAmountType TotalPrecioVenta = new SunatService.Modelos.TaxInclusiveAmountType();
                TotalPrecioVenta.currencyID = Comprobante.Idmoneda;
                TotalPrecioVenta.Value = Convert.ToDecimal(Comprobante.Total);
                //TotalValorVenta.TaxInclusiveAmount = TotalPrecioVenta;

                SunatService.Modelos.AllowanceTotalAmountType MtoTotalDsctos = new SunatService.Modelos.AllowanceTotalAmountType();
                MtoTotalDsctos.currencyID = Comprobante.Idmoneda;
                MtoTotalDsctos.Value = Convert.ToDecimal(Comprobante.TotDsctos);
                //TotalValorVenta.AllowanceTotalAmount = MtoTotalDsctos;

                //SunatService.Modelos.ChargeTotalAmountType MtoTotalOtrosCargos = new SunatService.Modelos.ChargeTotalAmountType();
                //MtoTotalOtrosCargos.currencyID = Comprobante.Idmoneda;
                //MtoTotalOtrosCargos.Value = Convert.ToDecimal(string.Format("{0:0.00}", Comprobante.TotOtros));
                //TotalValorVenta.ChargeTotalAmount = MtoTotalOtrosCargos;

                //SunatService.Modelos.PrepaidAmountType MtoCargos = new SunatService.Modelos.PrepaidAmountType();
                //MtoCargos.currencyID = Comprobante.Idmoneda;
                //MtoCargos.Value = Convert.ToDecimal(string.Format("{0:0.00}", Comprobante.TotOtros));
                ////MtoCargos.Value = Convert.ToDecimal(string.Format("{0:0.00}", 0));
                //TotalValorVenta.PrepaidAmount = MtoCargos;

                SunatService.Modelos.PayableAmountType ImporteTotalVenta = new SunatService.Modelos.PayableAmountType();
                ImporteTotalVenta.currencyID = Comprobante.Idmoneda;
                ImporteTotalVenta.Value = Convert.ToDecimal(string.Format("{0:0.00}", Comprobante.Total));

                TotalValorVenta.LineExtensionAmount = TValorVenta;
                //TotalValorVenta.TaxInclusiveAmount = TotalPrecioVenta;
                //TotalValorVenta.AllowanceTotalAmount = MtoTotalDsctos;
                //  TotalValorVenta.ChargeTotalAmount = MtoTotalOtrosCargos;
                //TotalValorVenta.PrepaidAmount = MtoCargos;
                TotalValorVenta.PayableAmount = ImporteTotalVenta;
                Factura.LegalMonetaryTotal = TotalValorVenta;

                //Número de orden del Ítem 
                //Cantidad y Unidad de medida por ítem 
                //Valor de venta del ítem
                //Items de Factura
                SunatService.Modelos.InvoiceLineType[] items = new SunatService.Modelos.InvoiceLineType[10];
                int iditem = 1;

                foreach (Detalles det in Comprobante.Detalles)
                {
                    SunatService.Modelos.InvoiceLineType item = new SunatService.Modelos.InvoiceLineType();
                    SunatService.Modelos.IDType numeroItem = new SunatService.Modelos.IDType();
                    numeroItem.Value = iditem.ToString();
                    item.ID = numeroItem;

                    SunatService.Modelos.InvoicedQuantityType cantidad = new SunatService.Modelos.InvoicedQuantityType();
                    cantidad.unitCodeListAgencyName = "United Nations Economic Commission for Europe";
                    cantidad.unitCodeListID = "UN/ECE rec 20";
                    cantidad.unitCode = det.UnidadMedida;
                    item.InvoicedQuantity = cantidad;
                    cantidad.Value = Convert.ToInt32(det.Cantidad);
                    item.InvoicedQuantity = cantidad;

                    SunatService.Modelos.LineExtensionAmountType ValorVenta = new SunatService.Modelos.LineExtensionAmountType();
                    ValorVenta.currencyID = Comprobante.Idmoneda;
                    ValorVenta.Value = Convert.ToDecimal(string.Format("{0:0.00}", det.Total));
                    item.LineExtensionAmount = ValorVenta;

                    //Precio de venta unitario por item y código 
                    SunatService.Modelos.PricingReferenceType ValorReferenUnitario = new SunatService.Modelos.PricingReferenceType();
                    //ValorReferenUnitario.AlternativeConditionPrice
                    SunatService.Modelos.PriceType[] TipoPrecios = new SunatService.Modelos.PriceType[2];
                    SunatService.Modelos.PriceType TipoPrecio = new SunatService.Modelos.PriceType();

                    SunatService.Modelos.PriceAmountType PrecioMonto = new SunatService.Modelos.PriceAmountType();

                    PrecioMonto.currencyID = Comprobante.Idmoneda;
                    PrecioMonto.Value = Convert.ToDecimal(string.Format("{0:0.000}", det.Precio));
                    TipoPrecio.PriceAmount = PrecioMonto;

                    SunatService.Modelos.PriceTypeCodeType TipoPrecioCode = new SunatService.Modelos.PriceTypeCodeType();
                    TipoPrecioCode.listName = "Tipo de Precio";
                    TipoPrecioCode.listAgencyName = "PE:SUNAT";
                    TipoPrecioCode.listURI = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo16";
                    TipoPrecioCode.Value = "01";


                    TipoPrecio.PriceTypeCode = TipoPrecioCode;
                    TipoPrecios[0] = TipoPrecio;
                    ValorReferenUnitario.AlternativeConditionPrice = TipoPrecios;
                    item.PricingReference = ValorReferenUnitario;

                    SunatService.Modelos.TaxTotalType[] Totales_Items = new SunatService.Modelos.TaxTotalType[2];
                    SunatService.Modelos.TaxTotalType Totales_Item = new SunatService.Modelos.TaxTotalType();

                    SunatService.Modelos.TaxAmountType Total_Item = new SunatService.Modelos.TaxAmountType();
                    Total_Item.currencyID = Comprobante.Idmoneda;
                    Total_Item.Value = Convert.ToDecimal(string.Format("{0:0.00}", det.mtoValorVentaItem - (det.mtoValorVentaItem / 1.18m)));
                    Totales_Item.TaxAmount = Total_Item;

                    SunatService.Modelos.TaxSubtotalType[] subtotal_Items = new SunatService.Modelos.TaxSubtotalType[2];
                    SunatService.Modelos.TaxSubtotalType subtotal_Item = new SunatService.Modelos.TaxSubtotalType();

                    SunatService.Modelos.TaxableAmountType taxsubtotal_IGVItem = new SunatService.Modelos.TaxableAmountType();
                    taxsubtotal_IGVItem.currencyID = Comprobante.Idmoneda;
                    taxsubtotal_IGVItem.Value = Convert.ToDecimal(string.Format("{0:0.00}", det.mtoValorVentaItem / 1.18m));
                    subtotal_Item.TaxableAmount = taxsubtotal_IGVItem;

                    SunatService.Modelos.TaxAmountType TotalTaxAmount_IGVItem = new SunatService.Modelos.TaxAmountType();
                    TotalTaxAmount_IGVItem.currencyID = Comprobante.Idmoneda;
                    TotalTaxAmount_IGVItem.Value = Convert.ToDecimal(string.Format("{0:0.00}", det.mtoValorVentaItem - (det.mtoValorVentaItem / 1.18m)));
                    subtotal_Item.TaxAmount = TotalTaxAmount_IGVItem;

                    subtotal_Items[0] = subtotal_Item;
                    Totales_Item.TaxSubtotal = subtotal_Items;

                    SunatService.Modelos.TaxCategoryType taxcategory_IGVItem = new SunatService.Modelos.TaxCategoryType();
                    //taxcategory_IGVItem.ID = id_IGVItem;
                    Modelos.IDType idTaxCategoria = new Modelos.IDType();
                    idTaxCategoria.schemeAgencyName = "United Nations Economic Commission for Europe";
                    idTaxCategoria.schemeName = "Tax Category Identifier";
                    idTaxCategoria.schemeID = "UN/ECE 5305";
                    idTaxCategoria.Value = "S";
                    //taxcategory_IGVItem.ID = idTaxCategoria;

                    PercentType1 porcentaje = new PercentType1();
                    porcentaje.Value = Convert.ToDecimal(det.porIgvItem) * 100;
                    taxcategory_IGVItem.Percent = porcentaje;
                    subtotal_Item.TaxCategory = taxcategory_IGVItem;
                    // taxcategory_IGVItem.Percent= taxcategory_IGVItem;

                    Modelos.TaxExemptionReasonCodeType ReasonCode = new Modelos.TaxExemptionReasonCodeType();
                    ReasonCode.listURI = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo07";
                    ReasonCode.listName = "Afectacion del IGV";
                    ReasonCode.listAgencyName = "PE:SUNAT";
                    ReasonCode.Value = "10";

                    taxcategory_IGVItem.TaxExemptionReasonCode = ReasonCode;

                    SunatService.Modelos.TaxSchemeType taxscheme_IGVItem = new SunatService.Modelos.TaxSchemeType();
                    SunatService.Modelos.IDType id2_IGVItem = new SunatService.Modelos.IDType();
                    id2_IGVItem.schemeID = "UN/ECE 5153";
                    id2_IGVItem.schemeAgencyID = "6";
                    id2_IGVItem.Value = "1000";
                    taxscheme_IGVItem.ID = id2_IGVItem;
                    //id2_IGVItem.schemeName = "Codigo de tributos";
                    //id2_IGVItem.schemeAgencyName = "PE:SUNAT";

                    SunatService.Modelos.NameType1 nombreImpto_IGVItem = new SunatService.Modelos.NameType1();
                    nombreImpto_IGVItem.Value = "IGV";
                    taxscheme_IGVItem.Name = nombreImpto_IGVItem;

                    SunatService.Modelos.TaxTypeCodeType nombreImpto_IGVItemInter = new SunatService.Modelos.TaxTypeCodeType();
                    nombreImpto_IGVItemInter.Value = "VAT";
                    taxscheme_IGVItem.TaxTypeCode = nombreImpto_IGVItemInter;
                    taxscheme_IGVItem.Name = nombreImpto_IGVItem;

                    //SunatService.Modelos.TaxExemptionReasonCodeType CodRazon_IGVItem = new SunatService.Modelos.TaxExemptionReasonCodeType();
                    //CodRazon_IGVItem.listName = "SUNAT: Codigo de Tipo de Afectación del IGV";
                    //CodRazon_IGVItem.listAgencyName = "PE:SUNAT";
                    //CodRazon_IGVItem.listURI = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo07";
                    //CodRazon_IGVItem.Value = "10";

                    //taxcategory_IGVItem.TaxExemptionReasonCode = CodRazon_IGVItem;
                    taxcategory_IGVItem.TaxScheme = taxscheme_IGVItem;
                    //subtotal_Item.TaxableAmount = taxsubtotal_IGVItem;

                    //subtotal_Item.TaxCategory = taxcategory_IGVItem;
                    //subtotal_Item.TaxAmount = TotalTaxAmount_IGVItem;

                    subtotal_Items[0] = subtotal_Item;
                    Totales_Item.TaxSubtotal = subtotal_Items;
                    Totales_Items[0] = Totales_Item;

                    item.TaxTotal = Totales_Items;

                    SunatService.Modelos.DescriptionType[] descriptions = new SunatService.Modelos.DescriptionType[2];
                    SunatService.Modelos.DescriptionType description = new SunatService.Modelos.DescriptionType();
                    description.Value = det.DescripcionProducto;
                    SunatService.Modelos.ItemIdentificationType codigoProd = new SunatService.Modelos.ItemIdentificationType();
                    SunatService.Modelos.IDType id = new SunatService.Modelos.IDType();
                    id.Value = det.Codcom;
                    codigoProd.ID = id;

                    SunatService.Modelos.PriceType PrecioProducto = new SunatService.Modelos.PriceType();
                    SunatService.Modelos.PriceAmountType PrecioMontoTipo = new SunatService.Modelos.PriceAmountType();
                    PrecioMontoTipo.Value = Convert.ToDecimal(string.Format("{0:0.00}", det.Precio / (det.porIgvItem + 1)));
                    //PrecioMontoTipo.Value = Convert.ToDecimal(string.Format("{0:0.00}", det.Precio));
                    PrecioMontoTipo.currencyID = Comprobante.Idmoneda;
                    PrecioProducto.PriceAmount = PrecioMontoTipo;

                    SunatService.Modelos.ItemType itemTipo = new SunatService.Modelos.ItemType();
                    descriptions[0] = description;
                    itemTipo.Description = descriptions;
                    itemTipo.SellersItemIdentification = codigoProd;

                    item.Item = itemTipo;
                    item.Price = PrecioProducto;
                    //Item_Adicional.AdditionalItemProperty = Propiedades;
                    //Item_Adicionales.Item = Item_Adicional;            

                    //items[1] = item_OpeOnerosa;
                    // items[2] = item_DsctoItem;
                    // items[3] = item_DsctoCargoItem;
                    //items[4] = item_IGVItem;
                    //items[5] = item_ISCItem;
                    // items[6] = Item_Descripcion;
                    //  items[7] = Item_CodProSUNAT;
                    //   items[8] = Item_Adicionales;
                    items[iditem] = item;
                    iditem += 1;
                }
                Factura.InvoiceLine = items; //Carga de los detalles del comprobante
                //
                string archXML = GenerarComprobante(Factura, Comprobante.EmpresaRUC, Comprobante.Idtipocomp, Comprobante.Serie, Comprobante.Numero);
                string rptFirma = FirmarXML(archXML, Ruta_Certificado, Password_Certificado);
                string strEnvio = Ruta_ENVIOS + Path.GetFileName(archXML).Replace(".xml", ".zip");
                Comprimir(archXML, strEnvio);

                SunatResponse resultado = new SunatResponse();
                resultado = EnviarDocumento(strEnvio);

                return resultado;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
                        
        private  string GenerarComprobante(SunatService.Modelos.InvoiceType Factura, 
                                                                             string RUC, 
                                                                             string TipoDocumento, string Serie,
                                                                             string Numero)
        {
            //-----Generando el archivo XML 
            XmlWriterSettings setting = new XmlWriterSettings();
            //Especificar el uso de sangrias para los etiquetas XML
            setting.Indent = true;
            setting.IndentChars = "\t";

            //Generar el nombre del archivo
            string RUCEmpresa =  RUC;
            string ArchivoXML = RUC + "-" + TipoDocumento + "-" + Serie + "-" + Numero;
           string rutaXML = string.Format(@"{0}{1}.xml", this.Ruta_XML , ArchivoXML);
 
            //Generar el xml en la ruta espeficificada
            using (XmlWriter writer = XmlWriter.Create(rutaXML, setting))
            {
                Type typeToSerialize = typeof(SunatService.Modelos.InvoiceType);
                XmlSerializer xs = new XmlSerializer(typeToSerialize);
                xs.Serialize(writer, Factura);
                return rutaXML;
            }
        }

        public  string FirmarXML(string cRutaArchivo, string cCertificado, string cClave)
        {

            string file = cRutaArchivo;
            //Corregir un error de xml en el archivo generado
            string text = File.ReadAllText(file);
            text = text.Replace(@"<ext:UBLExtension />", @"<ext:UBLExtension> <ext:ExtensionContent /></ext:UBLExtension>");
            text = text.Replace("xsi:type=", "");
            text = text.Replace("cbc:SerialIDType", "");
            text = text.Replace("\"\"", "");
            //Guardar las modificaciones
            File.WriteAllText(file, text);
            //Firmar el archivo xml
            string cRpta;
            string tipo = Path.GetFileName(cRutaArchivo);
            string local_typoDocumento = tipo.Substring(12, 2); // retorna 01 o 03 0 ...
            string l_xpath = "";
            string f_certificat = cCertificado;
            string f_pwd = cClave;
            string xmlFile = cRutaArchivo;

            System.Security.Cryptography.X509Certificates.X509Certificate2 MonCertificat = new System.Security.Cryptography.X509Certificates.X509Certificate2(f_certificat, f_pwd);
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.PreserveWhitespace = true;
            //Leer el xml a firmar
            xmlDoc.Load(xmlFile);
            //Firmar el documento xml
            SignedXml signedXml = new SignedXml(xmlDoc);
            signedXml.SigningKey = MonCertificat.PrivateKey;
            KeyInfo KeyInfo = new KeyInfo();
            Reference Reference = new Reference();
            Reference.Uri = "";
            Reference.AddTransform(new XmlDsigEnvelopedSignatureTransform());
            signedXml.AddReference(Reference);
            X509Chain X509Chain = new X509Chain();
            X509Chain.Build(MonCertificat);
            X509ChainElement local_element = X509Chain.ChainElements[0];
            KeyInfoX509Data x509Data = new KeyInfoX509Data(local_element.Certificate);
            string subjectName = local_element.Certificate.Subject;
            x509Data.AddSubjectName(subjectName);
            KeyInfo.AddClause(x509Data);
            signedXml.KeyInfo = KeyInfo;
            signedXml.ComputeSignature();
            XmlElement signature = signedXml.GetXml();
            signature.Prefix = "ds";
            signedXml.ComputeSignature();
            foreach (XmlNode node in signature.SelectNodes("descendant-or-self::*[namespace-uri()='http://www.w3.org/2000/09/xmldsig#']"))
            {
                // node.Prefix = "ds"
                if (node.LocalName == "Signature")
                {
                    XmlAttribute newAttribute = xmlDoc.CreateAttribute("Id");
                    newAttribute.Value = "SignSUNAT";
                    node.Attributes.Append(newAttribute);
                }
            }
            XmlNamespaceManager nsMgr;
            nsMgr = new XmlNamespaceManager(xmlDoc.NameTable);
            nsMgr.AddNamespace("sac", "urn:sunat:names:specification:ubl:peru:schema:xsd:SunatAggregateComponents-1");
            nsMgr.AddNamespace("ccts", "urn:un:unece:uncefact:documentation:2");
            nsMgr.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");

            switch (local_typoDocumento)
            {
                case "01":
                case "03" // factura y boleta
               :
                    {
                        nsMgr.AddNamespace("tns", "urn:oasis:names:specification:ubl:schema:xsd:Invoice-2");
                        l_xpath = "/tns:Invoice/ext:UBLExtensions/ext:UBLExtension[1]/ext:ExtensionContent";
                        break;
                    }

                case "07" // n ota de credito
         :
                    {
                        nsMgr.AddNamespace("tns", "urn:oasis:names:specification:ubl:schema:xsd:CreditNote-2");
                        l_xpath = "/tns:CreditNote/ext:UBLExtensions/ext:UBLExtension[1]/ext:ExtensionContent";
                        break;
                    }

                case "08" // nota de debito
                    :
                    {
                        nsMgr.AddNamespace("tns", "urn:oasis:names:specification:ubl:schema:xsd:DebitNote-2");
                        l_xpath = "/tns:DebitNote/ext:UBLExtensions/ext:UBLExtension[1]/ext:ExtensionContent";
                        break;
                    }

                case "RA" // COMUNICACION DE BAJA
                    :
                    {
                        nsMgr.AddNamespace("tns", "urn:sunat:names:specification:ubl:peru:schema:xsd:VoidedDocuments-1");
                        l_xpath = "/tns:VoidedDocuments/ext:UBLExtensions/ext:UBLExtension/ext:ExtensionContent";
                        break;
                    }

                case "RC" // RESUMEN DIARIO
                     :
                    {
                        nsMgr.AddNamespace("tns", "urn:sunat:names:specification:ubl:peru:schema:xsd:SummaryDocuments-1");
                        l_xpath = "/tns:SummaryDocuments/ext:UBLExtensions/ext:UBLExtension/ext:ExtensionContent";

                        break;
                    }
        
                default  // GUIA REMISION
                    :
                    {                       
                        nsMgr.AddNamespace("tns", "urn:oasis:names:specification:ubl:schema:xsd:DespatchAdvice-2");                    
                        l_xpath = "/tns:DespatchAdvice/ext:UBLExtensions/ext:UBLExtension[1]/ext:ExtensionContent";

                        break;
                    }
            }
            nsMgr.AddNamespace("cac", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
            nsMgr.AddNamespace("udt", "urn:un:unece:uncefact:data:specification:UnqualifiedDataTypesSchemaModule:2");
            nsMgr.AddNamespace("ext", "urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2");
            nsMgr.AddNamespace("qdt", "urn:oasis:names:specification:ubl:schema:xsd:QualifiedDatatypes-2");
            nsMgr.AddNamespace("cbc", "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2");
            nsMgr.AddNamespace("ds", "http://www.w3.org/2000/09/xmldsig#");
            xmlDoc.SelectSingleNode(l_xpath, nsMgr).AppendChild(xmlDoc.ImportNode(signature, true));
            //Guarda los cambios en el xml firmado
            xmlDoc.Save(xmlFile);
            XmlNodeList nodeList = xmlDoc.GetElementsByTagName("ds:Signature");
            if ((nodeList.Count != 1))
            {
                cRpta = "SE PRODUJO ERROR EN LA FIRMA";
            }
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //FIRMAR COMPROBANTE
            signedXml.LoadXml((XmlElement)nodeList[0]);
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            if ((signedXml.CheckSignature()) == false)
                cRpta = "SE PRODUJO UN ERROR EN LA FIRMA  DE DOCUMENTO";
            else
                cRpta = "OK";
            //string file = cRutaArchivo + cArchivo;              
            return cRpta;
        }

        public  string Comprimir(string cnombrearchivoOrigen, string cnombreArchivoDestino)
        {
            
            Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile();
            zip.AddFile(cnombrearchivoOrigen, ""); // se puede seguir agregando mas con a misma funcion
            zip.Save(cnombreArchivoDestino);
            string rpta = "OK";
            return rpta;
        }

        public SunatResponse EnviarDocumento(string pArchivo)
        {
            string filezip = pArchivo;
            string filepath = filezip;
            byte[] bitArray = File.ReadAllBytes(filepath);

            var response = new SunatResponse
            {
                Success = false
            };

            try
            {

                billServiceClient servicio = new billServiceClient(billServiceClient.EndpointConfiguration.BillServicePort);                

                ServicePointManager.UseNagleAlgorithm = true;
                ServicePointManager.Expect100Continue = false;
                ServicePointManager.CheckCertificateRevocationList = true;

                servicio.OpenAsync();
                //obtener el nombre del archivo sin ruta
                filezip = Path.GetFileName(filezip);
                var rpta = servicio.sendBill(filezip, bitArray, string.Empty);
                rpta.Wait();
                servicio.CloseAsync();

                var result = rpta.Result;
                using (var outputXml = ProcessZip.ExtractFile(result.applicationResponse))
                {
                    response = new SunatResponse
                    {
                        Success = true,
                        ApplicationResponse = ProcessXml.GetAppResponse(outputXml),
                        ContentZip = result.applicationResponse
                    };
                }

                filezip = Path.GetFileName(filezip);
                FileStream fs = new FileStream(Ruta_CDRS + "R-" + filezip, FileMode.Create);
                fs.Write(result.applicationResponse, 0, result.applicationResponse.Length);
                fs.Close();

            }
            catch (System.ServiceModel.FaultException ex)
            {
                response.Error = GetErrorFromFault(ex);
            }
            catch (Exception er)
            {
                response.Error = new ErrorResponse
                {
                    Description = er.Message
                };
            }

            return response;
        }

        public async Task<SunatResponse> ObtenerEstado(string pstrTicket)
        {
            var res = new SunatResponse();
            try
            {

                billServiceClient servicio = new billServiceClient(billServiceClient.EndpointConfiguration.BillServicePort);

                ///var service = ServiceHelper.GetService<billService>(_config, _url);

                var result = await servicio.getStatusAsync(pstrTicket);
                var response = result.status;
                switch (response.statusCode)
                {
                    case "0":
                    case "99":
                        res.Success = true;
                        using (var xmlCdr = ProcessZip.ExtractFile(response.content))
                            res.ApplicationResponse = ProcessXml.GetAppResponse(xmlCdr);

                        res.ContentZip = response.content;
                        break;
                    case "98":
                        res.Success = false;
                        res.Error = new ErrorResponse { Description = "En Proceso" };
                        break;
                }
            }
            catch ( System.ServiceModel.FaultException ex)
            {
                res.Error = GetErrorFromFault(ex);
            }
            catch (Exception er)
            {
                res.Error = new ErrorResponse
                {
                    Description = er.Message,
                };
            }
            return res;
        }
        private static ErrorResponse GetErrorFromFault(System.ServiceModel.FaultException ex)
        {
            var errMsg = ProcessXml.GetDescriptionError(ex.Code.Name);
            if (string.IsNullOrEmpty(errMsg))
            {
                var msg = ex.CreateMessageFault();
                if (msg.HasDetail)
                {
                    var dets = msg.GetReaderAtDetailContents();
                    errMsg = dets.ReadElementString(dets.Name);
                }
            }
            return new ErrorResponse
            {
                Code = ex.Code.Name,
                Description = errMsg
            };
        }
        //public  string ObtenerEstado(string ticket)
        //{
        //    string strRetorno = "";
        //    try
        //    {
        //        billServiceClient servicio = new billServiceClient(billServiceClient.EndpointConfiguration.BillServicePort);
        //        {

        //            servicio.OpenAsync();
        //            statusResponse returnstring = servicio.getStatus(ticket);  
        //            strRetorno = returnstring.statusCode;
        //            servicio.CloseAsync();
        //            return strRetorno;
        //        }
        //    }
        //    catch (System.ServiceModel.FaultException ex)
        //    {
        //        throw new Exception(ex.Code.Name);                
        //    } 
        // }
        //public async Task<SunatResponse> EnviarDocumento2(string pArchivo)
        //{
        //    string strRetorno = "";
        //    string filezip = pArchivo;
        //    //string filepath = Directory.GetCurrentDirectory() + "\\Envios\\" + filezip;
        //    string filepath = filezip;
        //    byte[] bitArray = File.ReadAllBytes(filepath);
        //    try
        //    {

        //        billServiceClient servicio = new billServiceClient(billServiceClient.EndpointConfiguration.BillServicePort);

        //        ServicePointManager.UseNagleAlgorithm = true;
        //        ServicePointManager.Expect100Continue = false;
        //        ServicePointManager.CheckCertificateRevocationList = true;

        //        await servicio.OpenAsync();

        //        //obtener el nombre del archivo sin ruta
        //        filezip = Path.GetFileName(filezip);
        //        //Enviar el archivo zipeado convertido a Bytes a la SUNAT                    
        //        var result = await servicio.sendBillAsync(filezip, bitArray, "");
        //        await servicio.CloseAsync();

        //        filezip = Path.GetFileName(filezip);
        //        FileStream fs = new FileStream(Ruta_CDRS + "R-" + filezip, FileMode.Create);
        //        fs.Write(result.applicationResponse, 0, result.applicationResponse.Length);
        //        fs.Close();
        //        strRetorno = "Archivo generado con exito";

        //    }
        //    catch (System.ServiceModel.FaultException ex)
        //    {
        //        strRetorno = ex.Code.Name;
        //        throw;
        //    }
        //    return strRetorno;
        //}

    }
}
  




