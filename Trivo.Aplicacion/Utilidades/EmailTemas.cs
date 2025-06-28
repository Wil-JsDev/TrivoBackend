namespace Trivo.Aplicacion.Utilidades;

public static class EmailTemas
{
    public static string RegistroDeUsuario(string usuario,string codigo)
    {
        return $@"<!DOCTYPE html>
      <html lang=""es"">
      <head>
        <meta charset=""UTF-8"">
        <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
        <title>Código de verificación - trivo</title>
        <style>
          @import url('https://fonts.googleapis.com/css2?family=Inter:wght@300;400;500;600;700;800&display=swap');
          
          * {{
            margin: 0;
            padding: 0;
            box-sizing: border-box;
          }}
          
          body {{
            font-family: 'Inter', system-ui, -apple-system, sans-serif;
            background: linear-gradient(135deg, #f8fafc 0%, #f1f5f9 50%, #e2e8f0 100%);
            min-height: 100vh;
            padding: 20px;
            line-height: 1.6;
          }}
          
          .email-container {{
            max-width: 600px;
            margin: 0 auto;
            background: #ffffff;
            border-radius: 24px;
            box-shadow: 0 25px 50px -12px rgba(0, 0, 0, 0.15);
            overflow: hidden;
            position: relative;
          }}
          
          /* Header Styles */
          .header {{
            background: linear-gradient(135deg, #8b5cf6 0%, #7c3aed 50%, #6d28d9 100%);
            padding: 48px 32px;
            color: white;
            position: relative;
            overflow: hidden;
          }}
          
          .header::before {{
            content: '';
            position: absolute;
            top: -50%;
            right: -20%;
            width: 200px;
            height: 200px;
            background: rgba(255, 255, 255, 0.1);
            border-radius: 50%;
            animation: float 6s ease-in-out infinite;
          }}
          
          .header::after {{
            content: '';
            position: absolute;
            bottom: -30%;
            left: -10%;
            width: 150px;
            height: 150px;
            background: rgba(255, 255, 255, 0.08);
            border-radius: 50%;
            animation: float 8s ease-in-out infinite reverse;
          }}
          
          @keyframes float {{
            0%, 100% {{ transform: translateY(0px) rotate(0deg); }}
            50% {{ transform: translateY(-20px) rotate(5deg); }}
          }}
          
          .logo-section {{
            display: flex;
            align-items: center;
            gap: 16px;
            margin-bottom: 32px;
            position: relative;
            z-index: 10;
          }}
          
          .logo {{
            width: 56px;
            height: 56px;
            background: rgba(255, 255, 255, 0.15);
            border-radius: 16px;
            display: flex;
            align-items: center;
            justify-content: center;
            backdrop-filter: blur(10px);
            border: 1px solid rgba(255, 255, 255, 0.2);
          }}
          
          .logo svg {{
            width: 28px;
            height: 28px;
          }}
          
          .brand-name {{
            font-size: 32px;
            font-weight: 800;
            letter-spacing: -0.02em;
          }}
          
          .brand-tagline {{
            font-size: 14px;
            color: #e9d5ff;
            font-weight: 500;
            margin-top: 4px;
          }}
          
          .header-title {{
            font-size: 42px;
            font-weight: 800;
            margin-bottom: 8px;
            position: relative;
            z-index: 10;
            line-height: 1.1;
          }}
          
          .header-subtitle {{
            font-size: 20px;
            color: #e9d5ff;
            font-weight: 500;
            position: relative;
            z-index: 10;
          }}
          
          /* Main Content */
          .content {{
            padding: 48px 32px;
          }}
          
          .verification-section {{
            text-align: center;
            margin-bottom: 40px;
          }}
          
          .icon-wrapper {{
            width: 80px;
            height: 80px;
            background: linear-gradient(135deg, #f3e8ff 0%, #e9d5ff 100%);
            border-radius: 24px;
            display: inline-flex;
            align-items: center;
            justify-content: center;
            margin-bottom: 24px;
            position: relative;
            box-shadow: 0 10px 25px rgba(139, 92, 246, 0.15);
          }}
          
          .icon-wrapper::before {{
            content: '';
            position: absolute;
            inset: 0;
            background: linear-gradient(135deg, #8b5cf6, #a855f7);
            border-radius: 24px;
            opacity: 0.1;
            animation: pulse 3s ease-in-out infinite;
          }}
          
          @keyframes pulse {{
            0%, 100% {{ opacity: 0.1; transform: scale(1); }}
            50% {{ opacity: 0.2; transform: scale(1.05); }}
          }}
          
          .icon-wrapper svg {{
            width: 40px;
            height: 40px;
            color: #8b5cf6;
            position: relative;
            z-index: 10;
          }}
          
          .main-title {{
            font-size: 32px;
            font-weight: 700;
            color: #1f2937;
            margin-bottom: 16px;
          }}
          
          .main-description {{
            font-size: 18px;
            color: #6b7280;
            max-width: 480px;
            margin: 0 auto;
            line-height: 1.7;
          }}
          
          /* Code Section */
          .code-section {{
            background: linear-gradient(135deg, #faf5ff 0%, #f3e8ff 100%);
            border-radius: 20px;
            padding: 40px 32px;
            margin-bottom: 40px;
            text-align: center;
            border: 2px solid #e9d5ff;
            position: relative;
            overflow: hidden;
          }}
          
          .code-section::before {{
            content: '';
            position: absolute;
            inset: 0;
            background: linear-gradient(135deg, #8b5cf6, #a855f7);
            opacity: 0.03;
            border-radius: 18px;
          }}
          
          .code-label {{
            display: inline-block;
            background: #8b5cf6;
            color: white;
            padding: 8px 20px;
            border-radius: 50px;
            font-size: 12px;
            font-weight: 600;
            text-transform: uppercase;
            letter-spacing: 0.1em;
            margin-bottom: 20px;
          }}
          
          .verification-code {{
            font-size: 48px;
            font-weight: 900;
            color: #6d28d9;
            letter-spacing: 0.2em;
            font-family: 'Courier New', monospace;
            margin-bottom: 16px;
            text-shadow: 0 2px 4px rgba(139, 92, 246, 0.1);
          }}
          
          .code-expiry {{
            display: flex;
            align-items: center;
            justify-content: center;
            gap: 8px;
            color: #8b5cf6;
            font-size: 14px;
            font-weight: 500;
          }}
          
          .code-expiry svg {{
            width: 16px;
            height: 16px;
          }}
          
          /* Instructions */
          .instructions {{
            background: linear-gradient(135deg, #f8fafc 0%, #f1f5f9 100%);
            border-radius: 16px;
            padding: 32px;
            margin-bottom: 32px;
            border: 1px solid #e2e8f0;
          }}
          
          .instructions-header {{
            display: flex;
            align-items: flex-start;
            gap: 16px;
            margin-bottom: 20px;
          }}
          
          .check-icon {{
            width: 32px;
            height: 32px;
            background: #10b981;
            border-radius: 12px;
            display: flex;
            align-items: center;
            justify-content: center;
            flex-shrink: 0;
          }}
          
          .check-icon svg {{
            width: 18px;
            height: 18px;
            color: white;
          }}
          
          .instructions-title {{
            font-size: 20px;
            font-weight: 700;
            color: #1f2937;
            margin-bottom: 16px;
          }}
          
          .steps-list {{
            list-style: none;
            padding: 0;
          }}
          
          .step-item {{
            display: flex;
            align-items: center;
            gap: 16px;
            margin-bottom: 12px;
            font-size: 16px;
            color: #4b5563;
          }}
          
          .step-number {{
            width: 28px;
            height: 28px;
            background: #8b5cf6;
            color: white;
            border-radius: 50%;
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: 12px;
            font-weight: 700;
            flex-shrink: 0;
          }}
          
          /* Security Alert */
          .security-alert {{
            background: linear-gradient(135deg, #fef3c7 0%, #fde68a 100%);
            border-left: 4px solid #f59e0b;
            border-radius: 12px;
            padding: 24px;
            margin-bottom: 32px;
          }}
          
          .alert-header {{
            display: flex;
            align-items: flex-start;
            gap: 12px;
            margin-bottom: 12px;
          }}
          
          .alert-icon {{
            width: 32px;
            height: 32px;
            background: #f59e0b;
            border-radius: 12px;
            display: flex;
            align-items: center;
            justify-content: center;
            flex-shrink: 0;
          }}
          
          .alert-icon svg {{
            width: 18px;
            height: 18px;
            color: white;
          }}
          
          .alert-title {{
            font-size: 16px;
            font-weight: 700;
            color: #92400e;
          }}
          
          .alert-text {{
            font-size: 14px;
            color: #b45309;
            line-height: 1.6;
          }}
          
          /* CTA Button */
          .cta-section {{
            text-align: center;
            margin-bottom: 32px;
          }}
          
          .cta-button {{
            display: inline-flex;
            align-items: center;
            gap: 12px;
            background: linear-gradient(135deg, #8b5cf6 0%, #7c3aed 50%, #6d28d9 100%);
            color: white;
            padding: 18px 36px;
            border-radius: 16px;
            font-size: 18px;
            font-weight: 700;
            text-decoration: none;
            box-shadow: 0 10px 25px rgba(139, 92, 246, 0.3);
            transition: all 0.3s ease;
            border: none;
            cursor: pointer;
          }}
          
          .cta-button:hover {{
            transform: translateY(-2px);
            box-shadow: 0 15px 35px rgba(139, 92, 246, 0.4);
          }}
          
          .cta-button svg {{
            width: 20px;
            height: 20px;
          }}
          
          /* Footer */
          .footer {{
            background: linear-gradient(135deg, #f8fafc 0%, #f1f5f9 100%);
            padding: 32px;
            border-top: 1px solid #e2e8f0;
            text-align: center;
          }}
          
          .footer-text {{
            font-size: 14px;
            color: #6b7280;
            margin-bottom: 16px;
          }}
          
          .footer-text a {{
            color: #8b5cf6;
            text-decoration: none;
            font-weight: 600;
          }}
          
          .footer-text a:hover {{
            text-decoration: underline;
          }}
          
          .footer-links {{
            display: flex;
            align-items: center;
            justify-content: center;
            gap: 24px;
            margin-bottom: 24px;
            font-size: 14px;
          }}
          
          .footer-links a {{
            color: #6b7280;
            text-decoration: none;
            font-weight: 500;
            transition: color 0.3s ease;
          }}
          
          .footer-links a:hover {{
            color: #8b5cf6;
          }}
          
          .footer-divider {{
            width: 4px;
            height: 4px;
            background: #d1d5db;
            border-radius: 50%;
          }}
          
          .copyright {{
            padding-top: 24px;
            border-top: 1px solid #e2e8f0;
          }}
          
          .copyright-text {{
            display: flex;
            align-items: center;
            justify-content: center;
            gap: 8px;
            font-size: 12px;
            color: #9ca3af;
          }}
          
          .copyright-text svg {{
            width: 16px;
            height: 16px;
          }}
          
          /* Responsive Design */
          @media (max-width: 640px) {{
            body {{
              padding: 16px;
            }}
            
            .email-container {{
              border-radius: 16px;
            }}
            
            .header {{
              padding: 32px 24px;
            }}
            
            .header-title {{
              font-size: 32px;
            }}
            
            .content {{
              padding: 32px 24px;
            }}
            
            .verification-code {{
              font-size: 36px;
            }}
            
            .footer {{
              padding: 24px;
            }}
            
            .footer-links {{
              flex-direction: column;
              gap: 12px;
            }}
            
            .footer-divider {{
              display: none;
            }}
          }}
        </style>
      </head>
      <body>
        <div class=""email-container"">
          <!-- Header -->
          <div class=""header"">
            <div class=""logo-section"">
              <div class=""logo"">
                <svg fill=""currentColor"" viewBox=""0 0 24 24"">
                  <path d=""M20.84 4.61a5.5 5.5 0 0 0-7.78 0L12 5.67l-1.06-1.06a5.5 5.5 0 0 0-7.78 7.78l1.06 1.06L12 21.23l7.78-7.78 1.06-1.06a5.5 5.5 0 0 0 0-7.78z""/>
                </svg>
              </div>
              <div>
                <div class=""brand-name"">trivo</div>
                <div class=""brand-tagline"">Bienvenido a Trivo {usuario}</div>
              </div>
            </div>
            
            <h1 class=""header-title"">¡Tu código está aquí! 🎉</h1>
            <p class=""header-subtitle"">Último paso para unirte a la comunidad</p>
          </div>

          <!-- Main Content -->
          <div class=""content"">
            <!-- Verification Section -->
            <div class=""verification-section"">
              <div class=""icon-wrapper"">
                <svg fill=""none"" stroke=""currentColor"" viewBox=""0 0 24 24"">
                  <path stroke-linecap=""round"" stroke-linejoin=""round"" stroke-width=""2"" d=""M9 12l2 2 4-4m5.618-4.016A11.955 11.955 0 0112 2.944a11.955 11.955 0 01-8.618 3.04A12.02 12.02 0 003 9c0 5.591 3.824 10.29 9 11.622 5.176-1.332 9-6.031 9-11.622 0-1.042-.133-2.052-.382-3.016z""/>
                </svg>
              </div>
              
              <h2 class=""main-title"">Verifica tu identidad</h2>
              <p class=""main-description"">
                Ingresa este código en la app para completar tu registro y comenzar a conocer personas increíbles
              </p>
            </div>

            <!-- Verification Code -->
            <div class=""code-section"">
              <div class=""code-label"">Código de verificación</div>
              <div class=""verification-code"">{codigo}</div>
              <div class=""code-expiry"">
                <svg fill=""none"" stroke=""currentColor"" viewBox=""0 0 24 24"">
                  <path stroke-linecap=""round"" stroke-linejoin=""round"" stroke-width=""2"" d=""M12 8v4l3 3m6-3a9 9 0 11-18 0 9 9 0 0118 0z""/>
                </svg>
                <span>Expira en 10 minutos</span>
              </div>
            </div>

            <!-- Instructions -->
            <div class=""instructions"">
              <div class=""instructions-header"">
                <div class=""check-icon"">
                  <svg fill=""none"" stroke=""currentColor"" viewBox=""0 0 24 24"">
                    <path stroke-linecap=""round"" stroke-linejoin=""round"" stroke-width=""2"" d=""M5 13l4 4L19 7""/>
                  </svg>
                </div>
                <div>
                  <h3 class=""instructions-title"">Pasos a seguir:</h3>
                  <ul class=""steps-list"">
                    <li class=""step-item"">
                      <span class=""step-number"">1</span>
                      <span>Abre la aplicación trivo en tu dispositivo</span>
                    </li>
                    <li class=""step-item"">
                      <span class=""step-number"">2</span>
                      <span>Ingresa el código de 6 dígitos</span>
                    </li>
                    <li class=""step-item"">
                      <span class=""step-number"">3</span>
                      <span>¡Comienza tu aventura de conexiones!</span>
                    </li>
                  </ul>
                </div>
              </div>
            </div>

            <!-- Security Alert -->
            <div class=""security-alert"">
              <div class=""alert-header"">
                <div class=""alert-icon"">
                  <svg fill=""none"" stroke=""currentColor"" viewBox=""0 0 24 24"">
                    <path stroke-linecap=""round"" stroke-linejoin=""round"" stroke-width=""2"" d=""M12 9v2m0 4h.01m-6.938 4h13.856c1.54 0 2.502-1.667 1.732-2.5L13.732 4c-.77-.833-1.964-.833-2.732 0L3.732 16.5c-.77.833.192 2.5 1.732 2.5z""/>
                  </svg>
                </div>
                <div>
                  <h4 class=""alert-title"">🔒 Protege tu cuenta</h4>
                </div>
              </div>
              <p class=""alert-text"">
                Este código es personal e intransferible. Nunca lo compartas con terceros. 
                El equipo de trivo jamás te solicitará este código por teléfono o email.
              </p>
            </div>

          <!-- Footer -->
          <div class=""footer"">
            <p class=""footer-text"">
              ¿No solicitaste este código? 
              <a href=""#"">Reporta actividad sospechosa</a>
            </p>
            
            <div class=""footer-links"">
              <a href=""#"">Centro de ayuda</a>
              <div class=""footer-divider""></div>
              <a href=""#"">Soporte técnico</a>
              <div class=""footer-divider""></div>
              <a href=""#"">Privacidad</a>
            </div>
            
            <div class=""copyright"">
              <div class=""copyright-text"">
                <svg fill=""currentColor"" viewBox=""0 0 24 24"">
                  <path d=""M20.84 4.61a5.5 5.5 0 0 0-7.78 0L12 5.67l-1.06-1.06a5.5 5.5 0 0 0-7.78 7.78l1.06 1.06L12 21.23l7.78-7.78 1.06-1.06a5.5 5.5 0 0 0 0-7.78z""/>
                </svg>
                <span>© 2024 trivo • Conectando corazones, creando historias</span>
              </div>
            </div>
          </div>
        </div>
      </body>
      </html>";
    }

    public static string OlvidarContrasena(string code, string nombreUsuario)
    {
        return null;
    }
    
}