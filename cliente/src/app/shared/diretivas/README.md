



<span>Phone 01 ({{ phoneValue01 }}):</span><br>
<input type="text" [(AlMaskValue)]="phoneValue01" [AlMask]="phoneMask01" ngModel>
<br><br>
<span>Phone 02 ({{ phoneValue02 }}):</span><br>
<input type="text" [(AlMaskValue)]="phoneValue02" [AlMask]="phoneMask02" [spKeepMask]="true" ngModel>



component


  public phoneValue01: string = '1231234567';
  public phoneValue02: string;
  public phoneMask01 = GeproMaskUtil.PHONE_MASK_GENERATOR;
  public phoneMask02 = GeproMaskUtil.DYNAMIC_PHONE_MASK_GENERATOR;