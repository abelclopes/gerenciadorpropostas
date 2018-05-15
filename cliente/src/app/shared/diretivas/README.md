



<span>Phone 01 ({{ phoneValue01 }}):</span><br>
<input type="text" [(spMaskValue)]="phoneValue01" [spMask]="phoneMask01" ngModel>
<br><br>
<span>Phone 02 ({{ phoneValue02 }}):</span><br>
<input type="text" [(spMaskValue)]="phoneValue02" [spMask]="phoneMask02" [spKeepMask]="true" ngModel>