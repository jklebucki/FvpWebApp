﻿
var datatableLanguage = {
    "processing": "Przetwarzanie...",
    "search": "Szukaj:",
    "lengthMenu": "Pokaż _MENU_ pozycji",
    "info": "Pozycje od _START_ do _END_ z _TOTAL_ łącznie",
    "infoEmpty": "Pozycji 0 z 0 dostępnych",
    "infoFiltered": "(filtrowanie spośród _MAX_ dostępnych pozycji)",
    "infoPostFix": "",
    "loadingRecords": "Wczytywanie...",
    "zeroRecords": "Nie znaleziono pasujących pozycji",
    "emptyTable": "Brak danych",
    "paginate": {
        "first": "Pierwsza",
        "previous": "Poprzednia",
        "next": "Następna",
        "last": "Ostatnia"
    },
    "aria": {
        "sortAscending": ": aktywuj, by posortować kolumnę rosnąco",
        "sortDescending": ": aktywuj, by posortować kolumnę malejąco"
    }
};

var documentData = {
    document: {
        documentId,
        externalId,
        sourceId,
        contractorId,
        taskTicketId,
        documentNumber,
        documentSymbol,
        saleDate,
        documentDate,
        net,
        gross,
        vat,
        jpkV7,
        documentStatus,
        docContractorId,
        docContractorName,
        docContractorVatId,
        docContractorCity,
        docContractorPostCode,
        docContractorCountryCode,
        docContractorStreetAndNumber,
        docContractorFirm,
        createdAt,
        updatedAt,
        updatedBy,
        documentVats: [
            {
                documentVatId,
                vatCode,
                vatValue,
                vatAmount,
                netAmount,
                grossAmount,
                vatTags,
                documentId,
            },
        ],
    },
    contractors: [
        {
            contractorId,
            contractorErpId,
            contractorErpPosition,
            sourceId,
            gusContractorEntriesCount,
            contractorSourceId,
            name,
            street,
            estateNumber,
            quartersNumber,
            city,
            postalCode,
            province,
            vatId,
            regon,
            phone,
            email,
            countryCode,
            firm,
            contractorStatus,
            checkDate,
            documents,
        },
    ],
}