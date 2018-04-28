import { Component, OnInit } from '@angular/core';
import { FileUploadModule } from 'ng5-fileupload';
import { Ng2ModalWindow } from 'ng2-modal-module';

@Component({
  selector: 'app-propostas',
  templateUrl: './propostas.component.html',
  styleUrls: ['./propostas.component.css']
})
export class PropostasComponent implements OnInit {
  modalId: string = 'modalId';
  constructor() { }

  ngOnInit() {
    Ng2ModalWindow.showModal(this.modalId, {
      title: 'Modal title',
      htmlContent: 'Modal content'
    });
  }

}
