import { UsuariosModel, PermissaoUsuario } from "..";

export class UsuarioBuilder {
    private id?: string;
    private nome?: string;
    private email?: string;
    private cpf?: string;
    private perfilUsuario?: UsuariosModel.PerfilEnum;
    private permissaoUsuario?: PermissaoUsuario;
    private permissaoNivel?: any;
    private dataNacimento?: Date;
    private dataAtualizacao?: Date;
    private dataCriacao?: Date;
    private excluido?: boolean

    constructor(){}
    public  setId(value: string): UsuarioBuilder {
        this.id = value;
        return this;
    }
    public get Id() {
        return this.id;
    }
    public  setNome(nome: string)
    {
        this.nome = nome;
    }
    public get Nome() {
        return this.nome;
    }
    public  setCpf(value: string): UsuarioBuilder {
        this.cpf = value;
        return this;
    }
    public get Cpf() {
        return this.cpf;
    }
    public setEmail(value: string): UsuarioBuilder {
        this.email = value;
        return this;
    }
    public get Email() {
        return this.email;
    }        
    public setPerfilUsuario(value: UsuariosModel.PerfilEnum): UsuarioBuilder {
        this.perfilUsuario = value;
        return this;
    }
    public get PerfilUsuario() {
        return this.permissaoUsuario;
    }
    public setPermissaoUsuario(value: PermissaoUsuario): UsuarioBuilder {
        this.permissaoUsuario = value;
        return this;
    }
    public get PermissaoUsuario() {
        return this.permissaoUsuario;
    }
    setPermissaoNivel(value: any): UsuarioBuilder {
        this.permissaoUsuario = value;
        return this;
    }
    get PermissaoNivel() {
        return this.permissaoUsuario;
    }
    public  setDataNascimento(value: Date): UsuarioBuilder {
        this.dataNacimento = value;
        return this;
    }
    public get DataNascimento() {
        return this.dataNacimento;
    }
    public setDataAtualizacao(value: Date): UsuarioBuilder {
        this.dataAtualizacao = value;
        return this;
    }
    public get DataAtualizacao() {
        return this.dataAtualizacao;
    }
    public setDataCriacao(value: Date): UsuarioBuilder {
        this.dataCriacao = value;
        return this;
    }
    public get DataCriacao() {
        return this.dataCriacao;
    }
    public setExcluido(value: boolean): UsuarioBuilder {
        this.excluido = value;
        return this;
    }
    public get Excluido() {
        return this.excluido;
    }

    public build(): Usuario {
        return new Usuario(this);
    }
}

export class Usuario {
    private id?: string;
    private nome?: string;
    private cpf?: string;
    private email?: string;
    private perfilUsuario?: any;
    private permissaoUsuario?: PermissaoUsuario;
    private permissaoNivel?: any;
    private dataNacimento?: Date;
    private dataAtualizacao?: Date;
    private dataCriacao?: Date;
    private excluido?: boolean

    constructor(builder: UsuarioBuilder) {
        this.nome = builder.Nome;
        this.email = builder.Email;
        this.cpf = builder.Cpf;
        this.perfilUsuario = builder.PerfilUsuario;
        this.permissaoUsuario = builder.PermissaoUsuario;
        this.permissaoNivel = builder.PermissaoNivel;
        this.dataNacimento = builder.DataNascimento;
        this.id = builder.Id;
        this.dataAtualizacao = builder.DataAtualizacao;
        this.dataCriacao = builder.DataCriacao;
        this.excluido = builder.Excluido;
    }

    get Id() {
        return this.id;
    }
    get Nome() {
        return this.nome;
    }
    get Cpf() {
        return this.cpf;
    }
    get Email() {
        return this.email;
    } 
    get PerfilUsuario() {
        return this.permissaoUsuario;
    }
    get PermissaoUsuario() {
        return this.permissaoUsuario;
    }
    get PermissaoNivel() {
        return this.permissaoUsuario;
    }
    get DataNascimento() {
        return this.dataNacimento;
    }
    get DataAtualizacao() {
        return this.dataAtualizacao;
    }
    get DataCriacao() {
        return this.dataCriacao;
    }
    get Excluido() {
        return this.excluido;
    }
}

