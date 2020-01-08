using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlankApp1.Reports
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }

        public static List<Customer> GetCustomers()
        {
            return new List<Customer>
            {
                new Customer { CustomerId=4504,CustomerName="ALEC ENGINEERING & CONTRACTING LLC"},
                new Customer { CustomerId=4523,CustomerName="AL FUTTAIM CARILLION L.L.C"},
                new Customer { CustomerId=4533,CustomerName="AL HABTOOR LEIGHTON GROUP LLC"},
                new Customer { CustomerId=4571,CustomerName="AL JABER ENERGY SERVICES"},
                new Customer { CustomerId=4617,CustomerName="AL MASHOOB CONT"},
                new Customer { CustomerId=4651,CustomerName="AL NABOODAH CONTRACTING (L.L.C) COMPANY"},
                new Customer { CustomerId=4654,CustomerName="AL NABOODAH CONTRACTING (LLC)"},
                new Customer { CustomerId=4689,CustomerName="AL RAJHI CONSTRUCTION LLC"},
                new Customer { CustomerId=4761,CustomerName="ARABIAN CONSTRUCTION CO."},
                new Customer { CustomerId=4769,CustomerName="ARABTEC CONSTRUCTION LLC (DXB)"},
                new Customer { CustomerId=4805,CustomerName="AUSTRALIAN PILING TECHNOLOGY"},
                new Customer { CustomerId=4806,CustomerName="AUTOMAX DECORATION & GENERAL CONTG"},
                new Customer { CustomerId=4819,CustomerName="BAM HIGGS & HILL LLC."},
                new Customer { CustomerId=4825,CustomerName="BAUER INTERNATIONAL"},
                new Customer { CustomerId=4929,CustomerName="CIVILCO ENGINEERING AND CONTRACTING"},
                new Customer { CustomerId=4977,CustomerName="DARWISH ENGINEERING."},
                new Customer { CustomerId=4992,CustomerName="DESCON ENGINEERING ABU DHABI "},
                new Customer { CustomerId=5016,CustomerName="DUBAI CONTRACTING CO.L.L.C."},
                new Customer { CustomerId=5021,CustomerName="DUBAI SAUDI ARABIAN CONT.CO."},
                new Customer { CustomerId=5024,CustomerName="DBB Contracting L.L.C"},
                new Customer { CustomerId=5108,CustomerName="FIBREX LLC"},
                new Customer { CustomerId=5139,CustomerName="GHANTOOT TRANSPORT & GENERAL CONTRACTING ESTABLISHMENT (Building Division)"},
                new Customer { CustomerId=5140,CustomerName="GHANTOOT TRANSPORT & GENERAL CONTRACTING.(Roads Division)"},
                new Customer { CustomerId=5294,CustomerName="LARSEN & TOUBRO LIMITED"},
                new Customer { CustomerId=5324,CustomerName="MAX STEEL-SAUDI"},
                new Customer { CustomerId=5357,CustomerName="MOH. ABDULMOHSIN - (AUH) AL-KHARAFI & SONS"},
                new Customer { CustomerId=5447,CustomerName="NSCC INTERNATIONAL LIMITED."},
                new Customer { CustomerId=5449,CustomerName="NUROL "},
                new Customer { CustomerId=5466,CustomerName="ORASCOM CONTRACK"},
                new Customer { CustomerId=5493,CustomerName="POLENSKY & ZOELLNER ABUDHABI"},
                new Customer { CustomerId=5638,CustomerName="SITE TECHNOLOGY LTD CO."},
                new Customer { CustomerId=5639,CustomerName="SIX CONSTRUCT COMPANY LTD "},
                new Customer { CustomerId=5661,CustomerName="SQUARE GENERAL CONTRACTING CO. LLC"},
                new Customer { CustomerId=5670,CustomerName="STRABAG DUBAI L.L.C."},
                new Customer { CustomerId=5708,CustomerName="TECHNICAL ARCHITECT CONTRACTING LLC (AUH)"},
                new Customer { CustomerId=5767,CustomerName="TROJAN GENERAL CONTG."},
                new Customer { CustomerId=5797,CustomerName="WADE ADAMS CONTRTNG.L.L.C."},
                new Customer { CustomerId=5826,CustomerName="ZUBLIN CIVIL ENGINEERING CONTRACTORS."},
                new Customer { CustomerId=5859,CustomerName="TOA CORPORATION"},
                new Customer { CustomerId=5877,CustomerName="SHAPOORJI PALLONJI MIDEAST LLC"},
                new Customer { CustomerId=5925,CustomerName="GHANTOOT DUBAI"},
                new Customer { CustomerId=5926,CustomerName="ARCADE STAR CONSTRUCTION LLC"},
                new Customer { CustomerId=5933,CustomerName="LINDENBERG-EMIRATES LLC"},
                new Customer { CustomerId=5973,CustomerName="DODSAL ENGINEERING & CONSTRUCTION"},
                new Customer { CustomerId=6096,CustomerName="NEW YEAR CONTRACTION AND GENERAL MAINTENANCE"},
                new Customer { CustomerId=6134,CustomerName="AL FARA'A GROUP"},
                new Customer { CustomerId=6167,CustomerName="CONSOLIDATED CONTRACTING INTERNATIONAL COMPANY (CCC)"},
                new Customer { CustomerId=6224,CustomerName="FRAYLAND CONSTRUCTION & INTERIORS"},
                new Customer { CustomerId=6624,CustomerName="TARGET ENGINEERING CONSTRUCTION COMPANY LLC"},
                new Customer { CustomerId=7223,CustomerName="LAING O'ROURKE MIDDLE EAST (Holdings) LTD"},
                new Customer { CustomerId=7789,CustomerName="LEIGHTON MIDDLE EAST"},
                new Customer { CustomerId=7866,CustomerName="ARABTEC SAN JOSE JV. "},
                new Customer { CustomerId=8029,CustomerName="SAMSUNG ENGINEERING COMPANY LTD"},
                new Customer { CustomerId=8197,CustomerName="MCLAREN GROUP"},
                new Customer { CustomerId=8375,CustomerName="AL GHANDI - CCIC"},
                new Customer { CustomerId=8444,CustomerName="ALI MOUSA & SONS CONTRACTING"},
                new Customer { CustomerId=8706,CustomerName="GUNAL CONSTRUCTION TRADING"},
                new Customer { CustomerId=8888,CustomerName="Noufal"},
                new Customer { CustomerId=8933,CustomerName="LARSEN & TOUBRO (OMAN) LLC"},
                new Customer { CustomerId=9752,CustomerName="AUSPIC SUNSHINE FZE"},
                new Customer { CustomerId=9836,CustomerName="AL BAIDA PALACE FACTORY"},
                new Customer { CustomerId=10104,CustomerName="NATIONAL CONTRACTING & TRANSPORT "},
                new Customer { CustomerId=10529,CustomerName="CCC GROUP SAL - KUWAIT "},
                new Customer { CustomerId=10607,CustomerName="BATCO - NAEL & BIN HARMAL JV"},
                new Customer { CustomerId=10934,CustomerName="SUN FLOWER FZE"},
                new Customer { CustomerId=11219,CustomerName="AL RAJHI PROJECTS & CONSTRUCTION LLC"},
                new Customer { CustomerId=11246,CustomerName="SUN FLOWER"},
                new Customer { CustomerId=11312,CustomerName="BESIX - LARSEN & TOUBRO LTD"},
                new Customer { CustomerId=11453,CustomerName="ALI MOUSA & SONS CONTG"},
                new Customer { CustomerId=11463,CustomerName="BEAVER GULF CONTRACTING LLC"},
                new Customer { CustomerId=11726,CustomerName="SSANYONG & TROJAN JV"},
                new Customer { CustomerId=12675,CustomerName="TWAM AGRICULTURE LLC "},
                new Customer { CustomerId=13156,CustomerName="ORASCOM CONT & AL SAHRAA JV"}

                };
        }
    }
}
