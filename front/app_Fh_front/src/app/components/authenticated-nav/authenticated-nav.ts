import { Component, inject } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-authenticated-nav',
  imports: [RouterLink],
  templateUrl: './authenticated-nav.html',
})
export class AuthenticatedNav {
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  protected logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
