
export class PermissaoUsuario {
    constructor(
        public permissao?: string,
        public nivel?: number,
        public id?: string,
        public dataCriacao?: Date,
        public dataAtualizacao?: Date,
        public excluido?: boolean,
    ){}
}
