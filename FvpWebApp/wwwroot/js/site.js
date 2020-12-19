Vue.config.devtools = true
var datatableLanguage = {
  processing: "Przetwarzanie...",
  search: "Szukaj:",
  lengthMenu: "Pokaż _MENU_ pozycji",
  info: "Pozycje od _START_ do _END_ z _TOTAL_ łącznie",
  infoEmpty: "Pozycji 0 z 0 dostępnych",
  infoFiltered: "(filtrowanie spośród _MAX_ dostępnych pozycji)",
  infoPostFix: "",
  loadingRecords: "Wczytywanie...",
  zeroRecords: "Nie znaleziono pasujących pozycji",
  emptyTable: "Brak danych",
  paginate: {
    first: "Pierwsza",
    previous: "Poprzednia",
    next: "Następna",
    last: "Ostatnia",
  },
  aria: {
    sortAscending: ": aktywuj, by posortować kolumnę rosnąco",
    sortDescending: ": aktywuj, by posortować kolumnę malejąco",
  },
};

function decodeTicketType(type) {
  type = Number(type);
  switch (type) {
    case 0:
      return "Import dokumentów";
    case 1:
      return "Import kontrahentów";
    case 2:
      return "Sprawdzanie kontrahentów";
    case 3:
      return "Łączenie sprawdzonych kontrahentów z dokumentami";
    case 4:
      return "Zakładanie nowych kontrahentów w FK";
    case 5:
      return "Eksport dokumentów do FK";
    default:
      return "typ procesu nieznany";
  }
}

function decodeTicketStatus(status) {
  status = Number(status);
  switch (status) {
    case 0:
      return "Oczekujący na wykonanie";
    case 1:
      return "Procesowany...";
    case 2:
      return "Wykonany";
    case 3:
      return "Zakończony błędem";
    default:
      return "typ procesu nieznany";
  }
}

var contractorData = {
  contractorId: 0,
  contractorErpId: 0,
  contractorErpPosition: 0,
  sourceId: 0,
  gusContractorEntriesCount: 0,
  contractorSourceId: 0,
  name: 0,
  street: 0,
  estateNumber: 0,
  quartersNumber: 0,
  city: 0,
  postalCode: 0,
  province: 0,
  vatId: 0,
  regon: 0,
  phone: 0,
  email: 0,
  countryCode: 0,
  firm: 0,
  contractorStatus: 0,
  checkDate: 0,
  documents: 0,
};

var documentInfo = {
  document: {
    documentId: 0,
    externalId: 0,
    sourceId: 0,
    contractorId: 0,
    taskTicketId: 0,
    documentNumber: 0,
    documentSymbol: 0,
    saleDate: 0,
    documentDate: 0,
    net: 0,
    gross: 0,
    vat: 0,
    jpkV7: 0,
    documentStatus: 0,
    docContractorId: 0,
    docContractorName: 0,
    docContractorVatId: 0,
    docContractorCity: 0,
    docContractorPostCode: 0,
    docContractorCountryCode: 0,
    docContractorStreetAndNumber: 0,
    docContractorFirm: 0,
    createdAt: 0,
    updatedAt: 0,
    updatedBy: 0,
    documentVats: [
      {
        documentVatId: 0,
        vatCode: 0,
        vatValue: 0,
        vatAmount: 0,
        netAmount: 0,
        grossAmount: 0,
        vatTags: 0,
        documentId: 0,
      },
    ],
  },
  contractors: [contractorData],
};
var targetConfig = {
  target: {
    targetId: 0,
    description: null,
    databaseAddress: null,
    databaseName: null,
    databaseUsername: null,
    databasePassword: null,
  },
};
var vatRegister = {
  targetDocumentSettingsId: 0,
  sourceId: 0,
  documentShortcut: null,
  vatRegisterId: 0,
};
var accountingRecord = {
  account: null,
  accountingRecordId: 0,
  credit: null,
  debit: null,
  recordOrder: 0,
  sign: null,
  sourceId: 0,
};
var source = {
  sourceId: 0,
  targetId: null,
  description: null,
  code: null,
  type: null,
  address: null,
  dbName: null,
  username: null,
  password: null,
  accountingRecords: [accountingRecord],
};
var target = {
  targetId: 0,
  description: null,
  databaseAddress: null,
  databaseName: null,
  databaseUsername: null,
  databasePassword: null,
};
var targetDocumentSettings = {
  targetDocumentSettingsId: 0,
  sourceId: 0,
  documentShortcut: null,
  vatRegisterId: 0,
};
var sourceConfig = {
  source,
  target,
  targetDocumentSettings,
  vatRegisters: [vatRegister],
};
