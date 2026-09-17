import { Component, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { AuthenticatedNav } from '../../components/authenticated-nav/authenticated-nav';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-home',
  imports: [AuthenticatedNav, RouterLink],
  templateUrl: './home.html',
})
export class Home {
  protected readonly displayName = inject(AuthService).getDisplayName();
}
