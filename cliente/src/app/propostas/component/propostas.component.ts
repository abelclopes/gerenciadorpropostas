import { Component, OnInit } from '@angular/core';
import { FileUploadModule } from 'ng5-fileupload';
import { DialogService } from 'ng2-bootstrap-modal';
import { DialogModalComponent } from '../../shared/dialog-modal/dialog-modal.component';

@Component({
  selector: 'app-propostas',
  templateUrl: './propostas.component.html',
  styleUrls: ['./propostas.component.css']
})
export class PropostasComponent implements OnInit {
  modalId: string = 'modalId';
  constructor(private dialogService:DialogService) { }

  ngOnInit() {

  }
  showConfirm() {
    let disposable = this.dialogService.addDialog(DialogModalComponent, {
        title:'Confirm title',
        message:'Confirm message'})
        .subscribe((isConfirmed)=>{
            //We get dialog result
            if(isConfirmed) {
                alert('accepted');
            }
            else {
                alert('declined');
            }
        });
    //We can close dialog calling disposable.unsubscribe();
    //If dialog was not closed manually close it by timeout
    setTimeout(()=>{
        disposable.unsubscribe();
    },10000);
}
}
