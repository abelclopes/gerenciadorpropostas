
export class PropostaAnexoModel {
    constructor(
      public contentType?: string,
      public fileContent?: string,
      public file?: File,
      public nome?: string,
      public anexo?: string,
      public id?: string,
      public dataAtualizacao?: Date,
      public dataCriacao?: Date,
      public excluido?: boolean
    ){}
  }