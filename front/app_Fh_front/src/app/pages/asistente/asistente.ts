import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AuthenticatedNav } from '../../components/authenticated-nav/authenticated-nav';
import { AsistenteChatMessage } from '../../models/asistente-chat-message';
import { AsistenteService } from '../../services/asistente.service';

@Component({
  selector: 'app-asistente',
  imports: [AuthenticatedNav, FormsModule],
  templateUrl: './asistente.html',
})
export class Asistente {
  private static readonly chatHistoryKey = 'fh.asistente-chat-history';
  private readonly asistenteService = inject(AsistenteService);

  protected message = '';
  protected readonly messages = signal<AsistenteChatMessage[]>(this.restoreMessages());
  protected readonly loading = signal(false);
  protected readonly error = signal<string | null>(null);

  protected send(): void {
    const mensaje = this.message.trim();
    if (!mensaje || this.loading()) return;

    this.addMessage({ contenido: mensaje, autor: 'usuario' });
    this.message = '';
    this.error.set(null);
    this.loading.set(true);

    this.asistenteService.chat({ mensaje }).subscribe({
      next: ({ respuesta }) => {
        this.addMessage({ contenido: respuesta, autor: 'asistente' });
        this.loading.set(false);
      },
      error: () => {
        this.error.set('No se pudo obtener una respuesta. Inténtalo de nuevo más tarde.');
        this.loading.set(false);
      },
    });
  }

  private addMessage(message: AsistenteChatMessage): void {
    this.messages.update(messages => [...messages, message]);

    try {
      sessionStorage.setItem(Asistente.chatHistoryKey, JSON.stringify(this.messages()));
    } catch {
      // The conversation remains available in memory if browser storage is unavailable.
    }
  }

  private restoreMessages(): AsistenteChatMessage[] {
    try {
      const storedHistory = sessionStorage.getItem(Asistente.chatHistoryKey);
      if (!storedHistory) return [];

      const history: unknown = JSON.parse(storedHistory);
      if (Array.isArray(history) && history.every(Asistente.isChatMessage)) return history;

      sessionStorage.removeItem(Asistente.chatHistoryKey);
    } catch {
      try {
        sessionStorage.removeItem(Asistente.chatHistoryKey);
      } catch {
        // Storage can be unavailable or disabled by the browser.
      }
    }

    return [];
  }

  private static isChatMessage(message: unknown): message is AsistenteChatMessage {
    return typeof message === 'object'
      && message !== null
      && 'contenido' in message
      && typeof message.contenido === 'string'
      && 'autor' in message
      && (message.autor === 'usuario' || message.autor === 'asistente');
  }
}
