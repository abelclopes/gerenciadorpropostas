
export class UsuariosClans{    
    constructor(
        public Id?: string,
        public Nome?: string,
        public Email?: string,
        public Cpf?: string,
        public perfil?: any,
        public DataNacimento?: Date,
        public Idade?: number,
        public Excluido ?: boolean
    ){}
}