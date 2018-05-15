import { MaskGenerator } from "./interfaces/mask-generator.interface";

export class GeproMaskUtilService {

    private static PHONE_SMALL = '(999) 999-9999';
    private static PHONE_BIG = '(999) 9999-9999';
    private static CPF = '999.999.999-99';
    private static CNPJ = '99.999.999/9999-99';

    public static PHONE_MASK_GENERATOR: MaskGenerator = {
        generateMask: () =>  GeproMaskUtilService.PHONE_SMALL,
    }

    public static DYNAMIC_PHONE_MASK_GENERATOR: MaskGenerator = {
        generateMask: (value: string) => {
            return GeproMaskUtilService.hasMoreDigits(value, GeproMaskUtilService.PHONE_SMALL) ? 
                GeproMaskUtilService.PHONE_BIG : 
                GeproMaskUtilService.PHONE_SMALL;
        },
    }

    public static CPF_MASK_GENERATOR: MaskGenerator = {
        generateMask: () => GeproMaskUtilService.CPF,
    }

    public static CNPJ_MASK_GENERATOR: MaskGenerator = {
        generateMask: () => GeproMaskUtilService.CNPJ,
    }

    public static PERSON_MASK_GENERATOR: MaskGenerator = {
        generateMask: (value: string) => {
            return GeproMaskUtilService.hasMoreDigits(value, GeproMaskUtilService.CPF) ? 
                GeproMaskUtilService.CNPJ : 
                GeproMaskUtilService.CPF;
        },
    }

    private static hasMoreDigits(v01: string, v02: string): boolean {
        let d01 = this.onlyDigits(v01);
        let d02 = this.onlyDigits(v02);
        let len01 = (d01 && d01.length) || 0;
        let len02 = (d02 && d02.length) || 0;
        let moreDigits = (len01 > len02);
        return moreDigits;      
    }

    private static onlyDigits(value: string): string {
        let onlyDigits = (value != null) ? value.replace(/\D/g, '') : null;
        return onlyDigits;      
    }
}