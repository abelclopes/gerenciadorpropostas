export class CategoriaModel {
  constructor(
    public descricao?: string,
    public id?: string,
    public dataAtualizacao?: Date,
    public dataCriacao?: Date,
    public excluido?: boolean,
    public nome?: string,
){}
}
