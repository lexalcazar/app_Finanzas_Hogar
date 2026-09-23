import { provideHttpClient } from '@angular/common/http';
import { TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { Subject } from 'rxjs';
import { AsistenteChatResponse } from '../../models/asistente-chat-response';
import { AsistenteService } from '../../services/asistente.service';
import { Asistente } from './asistente';

describe('Asistente', () => {
  let response: Subject<AsistenteChatResponse>;
  let chat: ReturnType<typeof vi.fn>;

  beforeEach(() => {
    sessionStorage.clear();
    response = new Subject<AsistenteChatResponse>();
    chat = vi.fn(() => response.asObservable());
    TestBed.configureTestingModule({
      providers: [
        provideHttpClient(),
        provideRouter([]),
        { provide: AsistenteService, useValue: { chat } },
      ],
    });
  });

  function createFixture() {
    const fixture = TestBed.createComponent(Asistente);
    fixture.detectChanges();
    return fixture;
  }

  function send(fixture: ReturnType<typeof createFixture>, message: string): void {
    const component = fixture.componentInstance as unknown as { message: string; send(): void };
    component.message = message;
    component.send();
    fixture.detectChanges();
  }

  it('renders the initial chat state', () => {
    const fixture = createFixture();

    expect(fixture.nativeElement.textContent).toContain('Tu gestor de finanzas');
    expect(fixture.nativeElement.textContent).toContain('Escribe una pregunta para empezar.');
  });

  it('restores a valid conversation from the current browser session', () => {
    sessionStorage.setItem('fh.asistente-chat-history', JSON.stringify([
      { contenido: '¿Cuál es mi saldo?', autor: 'usuario' },
      { contenido: 'Tu saldo actual es 120 euros.', autor: 'asistente' },
    ]));

    const fixture = createFixture();

    expect(fixture.nativeElement.textContent).toContain('¿Cuál es mi saldo?');
    expect(fixture.nativeElement.textContent).toContain('Tu saldo actual es 120 euros.');
  });

  it('ignores and removes corrupt stored history', () => {
    sessionStorage.setItem('fh.asistente-chat-history', '{not-json');

    const fixture = createFixture();

    expect(fixture.nativeElement.textContent).toContain('Escribe una pregunta para empezar.');
    expect(sessionStorage.getItem('fh.asistente-chat-history')).toBeNull();
  });

  it('trims and sends a valid message, then adds both messages to the history', () => {
    const fixture = createFixture();

    send(fixture, '  ¿En qué gasté más?  ');
    expect(chat).toHaveBeenCalledWith({ mensaje: '¿En qué gasté más?' });
    expect(fixture.nativeElement.textContent).toContain('¿En qué gasté más?');

    response.next({ respuesta: 'El alquiler fue tu mayor gasto.' });
    response.complete();
    fixture.detectChanges();

    expect(fixture.nativeElement.textContent).toContain('El alquiler fue tu mayor gasto.');
    expect(fixture.nativeElement.textContent).toContain('¿En qué gasté más?');
    expect(fixture.nativeElement.querySelector('button').disabled).toBe(false);
    expect(JSON.parse(sessionStorage.getItem('fh.asistente-chat-history') ?? '[]')).toEqual([
      { contenido: '¿En qué gasté más?', autor: 'usuario' },
      { contenido: 'El alquiler fue tu mayor gasto.', autor: 'asistente' },
    ]);

    const restoredFixture = createFixture();
    expect(restoredFixture.nativeElement.textContent).toContain('El alquiler fue tu mayor gasto.');
  });

  it('does not send empty or whitespace-only messages', () => {
    const fixture = createFixture();

    send(fixture, '');
    send(fixture, '   ');

    expect(chat).not.toHaveBeenCalled();
    expect(fixture.nativeElement.textContent).toContain('Escribe una pregunta para empezar.');
  });

  it('shows loading and prevents duplicate requests while waiting', () => {
    const fixture = createFixture();

    send(fixture, '¿Cuál es mi saldo?');
    send(fixture, '¿Cuál es mi saldo?');

    expect(chat).toHaveBeenCalledTimes(1);
    expect(fixture.nativeElement.textContent).toContain('El asistente está procesando tu consulta...');
    expect((fixture.nativeElement.querySelector('.assistant-form button') as HTMLButtonElement).disabled).toBe(true);
    expect(JSON.parse(sessionStorage.getItem('fh.asistente-chat-history') ?? '[]')).toEqual([
      { contenido: '¿Cuál es mi saldo?', autor: 'usuario' },
    ]);
  });

  it('keeps the history, shows a generic error, and allows another send after failure', () => {
    const fixture = createFixture();

    send(fixture, '¿Qué gastos tengo?');
    response.error(new Error('network details'));
    fixture.detectChanges();

    expect(fixture.nativeElement.textContent).toContain('¿Qué gastos tengo?');
    expect(fixture.nativeElement.textContent).toContain('No se pudo obtener una respuesta. Inténtalo de nuevo más tarde.');
    expect(fixture.nativeElement.textContent).not.toContain('network details');
    expect((fixture.nativeElement.querySelector('.assistant-form button') as HTMLButtonElement).disabled).toBe(false);
    expect(JSON.parse(sessionStorage.getItem('fh.asistente-chat-history') ?? '[]')).toEqual([
      { contenido: '¿Qué gastos tengo?', autor: 'usuario' },
    ]);

    const nextResponse = new Subject<AsistenteChatResponse>();
    chat.mockReturnValueOnce(nextResponse.asObservable());
    send(fixture, '¿Y mis ingresos?');
    expect(chat).toHaveBeenCalledTimes(2);
  });
});
