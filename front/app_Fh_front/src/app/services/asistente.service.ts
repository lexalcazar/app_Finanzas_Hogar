import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { AsistenteChatRequest } from '../models/asistente-chat-request';
import { AsistenteChatResponse } from '../models/asistente-chat-response';

@Injectable({ providedIn: 'root' })
export class AsistenteService {
  private readonly http = inject(HttpClient);

  chat(request: AsistenteChatRequest) {
    return this.http.post<AsistenteChatResponse>(`${environment.apiUrl}/Asistente/chat`, request);
  }
}
