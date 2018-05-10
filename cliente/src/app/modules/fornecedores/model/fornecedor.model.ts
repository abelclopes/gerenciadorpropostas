export class FornecedorModel {
  constructor(
      public nome?: string,
      public cnpjCpf?: string,
      public email?: string,
      public telefone?: string,
      public id?: string,
      public dataAtualizacao?: Date,
      public dataCriacao?: Date,
      public excluido?: boolean,
  ){}
}
