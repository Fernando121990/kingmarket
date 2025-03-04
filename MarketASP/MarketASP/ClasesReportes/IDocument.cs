﻿using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketASP.ClasesReportes
{
    public interface IDocument
    {
        DocumentMetadata GetMetadata();
        DocumentSettings GetSettings();
        void Compose(IDocumentContainer container);
    }
}
