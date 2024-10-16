import { Component } from '@angular/core';
import { Router, RouterModule} from '@angular/router';
import { MatIconModule } from '@angular/material/icon';
import { MatToolbarModule} from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import {MatCardModule} from '@angular/material/card';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterModule, MatIconModule, MatToolbarModule, MatButtonModule, MatCardModule],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent {

  constructor(private router: Router){

  }

  title = 'TattooStudioManagement';


  redirection(rota:string){
    this.router.navigate([rota]);
  }
}

