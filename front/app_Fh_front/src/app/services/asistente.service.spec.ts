import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { AsistenteService } from './asistente.service';

describe('AsistenteService', () => {
  let service: AsistenteService;
  let httpTesting: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({ providers: [provideHttpClient(), provideHttpClientTesting()] });
    service = TestBed.inject(AsistenteService);
    httpTesting = TestBed.inject(HttpTestingController);
  });

  afterEach(() => httpTesting.verify());

  it('posts the message to the assistant endpoint and returns its response', () => {
    let respuesta = '';
    service.chat({ mensaje: '¿Cuál fue mi mayor gasto?' }).subscribe(response => respuesta = response.respuesta);

    const request = httpTesting.expectOne('/api/Asistente/chat');
    expect(request.request.method).toBe('POST');
    expect(request.request.body).toEqual({ mensaje: '¿Cuál fue mi mayor gasto?' });
    request.flush({ respuesta: 'Tu mayor gasto fue el alquiler.' });

    expect(respuesta).toBe('Tu mayor gasto fue el alquiler.');
  });
});
