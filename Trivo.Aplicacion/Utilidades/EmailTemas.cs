namespace Trivo.Aplicacion.Utilidades;

public static class EmailTemas
{
    public static string RegistroDeUsuario(string usuario,string codigo)
    {
      return @$"<!DOCTYPE html>
          <html lang=""es"">
          <head>
            <meta charset=""UTF-8"">
            <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
            <title>Código de verificación - trivo</title>
            <script src=""https://cdn.tailwindcss.com""></script>
            <script>
              tailwind.config = {{
                theme: {{
                  extend: {{
                    fontFamily: {{
                      'inter': ['Inter', 'system-ui', 'sans-serif'],
                    }},
                    animation: {{
                      'float': 'float 6s ease-in-out infinite',
                      'pulse-slow': 'pulse 3s ease-in-out infinite',
                    }}
                  }}
                }}
              }}
            </script>
            <link href=""https://fonts.googleapis.com/css2?family=Inter:wght@300;400;500;600;700;800&display=swap"" rel=""stylesheet"">
            <style>
              @keyframes float {{
                0%, 100% {{ transform: translateY(0px) rotate(0deg); }}
                50% {{ transform: translateY(-10px) rotate(2deg); }}
              }}
              
              .gradient-border {{
                background: linear-gradient(135deg, #8b5cf6, #a855f7, #c084fc);
                padding: 2px;
                border-radius: 20px;
              }}
              
              .gradient-border-inner {{
                background: white;
                border-radius: 18px;
              }}
              
              .glass-effect {{
                background: rgba(255, 255, 255, 0.1);
                backdrop-filter: blur(10px);
                border: 1px solid rgba(255, 255, 255, 0.2);
              }}
              
              .code-glow {{
                box-shadow: 0 0 30px rgba(139, 92, 246, 0.3);
              }}
            </style>
          </head>
          <body class=""font-inter bg-gradient-to-br from-slate-50 via-purple-50 to-indigo-50 min-h-screen"">
            
            <!-- Email Container -->
            <div class=""max-w-2xl mx-auto p-4 sm:p-8"">
              
              <!-- Main Email Card -->
              <div class=""bg-white rounded-3xl shadow-2xl overflow-hidden relative"">
                
                <!-- Floating Elements -->
                <div class=""absolute top-10 right-10 w-20 h-20 bg-gradient-to-br from-purple-400 to-pink-400 rounded-full opacity-20 animate-float""></div>
                <div class=""absolute top-32 left-8 w-12 h-12 bg-gradient-to-br from-blue-400 to-purple-400 rounded-full opacity-15 animate-float"" style=""animation-delay: 2s;""></div>
                <div class=""absolute bottom-20 right-16 w-16 h-16 bg-gradient-to-br from-indigo-400 to-purple-400 rounded-full opacity-10 animate-float"" style=""animation-delay: 4s;""></div>
                
                <!-- Header Section -->
                <div class=""relative bg-gradient-to-br from-purple-600 via-purple-700 to-indigo-800 px-8 py-12 overflow-hidden"">
                  
                  <!-- Background Pattern -->
                  <div class=""absolute inset-0 opacity-10"">
                    <div class=""absolute top-0 left-0 w-full h-full bg-gradient-to-br from-white/20 to-transparent""></div>
                    <div class=""absolute top-10 right-10 w-32 h-32 border border-white/20 rounded-full""></div>
                    <div class=""absolute bottom-10 left-10 w-24 h-24 border border-white/20 rounded-full""></div>
                  </div>
                  
                  <!-- Logo and Brand -->
                  <div class=""relative z-10"">
                    <div class=""flex items-center gap-4 mb-8"">
                      <div class=""w-14 h-14 glass-effect rounded-2xl flex items-center justify-center"">
                        <svg class=""w-8 h-8 text-white"" fill=""currentColor"" viewBox=""0 0 24 24"">
                          <path d=""M20.84 4.61a5.5 5.5 0 0 0-7.78 0L12 5.67l-1.06-1.06a5.5 5.5 0 0 0-7.78 7.78l1.06 1.06L12 21.23l7.78-7.78 1.06-1.06a5.5 5.5 0 0 0 0-7.78z""/>
                        </svg>
                      </div>
                      <div>
                        <h1 class=""text-3xl font-bold text-white tracking-tight"">trivo</h1>
                        <p class=""text-purple-200 text-sm font-medium"">Conectando corazones</p>
                      </div>
                    </div>
                    
                    <div class=""space-y-3"">
                      <h2 class=""text-4xl font-bold text-white leading-tight"">
                        {codigo}
                      </h2>
                      <p class=""text-purple-100 text-xl font-medium"">
                        Último paso para unirte a la comunidad
                      </p>
                    </div>
                  </div>
                </div>

                <!-- Main Content -->
                <div class=""px-8 py-12 relative"">
                  
                  <!-- Welcome Message -->
                  <div class=""text-center mb-12"">
                    <div class=""inline-flex items-center justify-center w-20 h-20 bg-gradient-to-br from-purple-100 to-indigo-100 rounded-3xl mb-6 relative"">
                      <div class=""absolute inset-0 bg-gradient-to-br from-purple-500 to-indigo-500 rounded-3xl opacity-10 animate-pulse-slow""></div>
                      <svg class=""w-10 h-10 text-purple-600 relative z-10"" fill=""none"" stroke=""currentColor"" viewBox=""0 0 24 24"">
                        <path stroke-linecap=""round"" stroke-linejoin=""round"" stroke-width=""2"" d=""M9 12l2 2 4-4m5.618-4.016A11.955 11.955 0 0112 2.944a11.955 11.955 0 01-8.618 3.04A12.02 12.02 0 003 9c0 5.591 3.824 10.29 9 11.622 5.176-1.332 9-6.031 9-11.622 0-1.042-.133-2.052-.382-3.016z""/>
                      </svg>
                    </div>
                    
                    <h3 class=""text-3xl font-bold text-gray-900 mb-4"">
                      Verifica tu identidad
                    </h3>
                    <p class=""text-gray-600 text-lg leading-relaxed max-w-md mx-auto"">
                      Ingresa este código en la app para completar tu registro y comenzar a conocer personas increíbles
                    </p>
                  </div>

                  <!-- Verification Code Card -->
                  <div class=""gradient-border mb-12 code-glow"">
                    <div class=""gradient-border-inner p-8 text-center"">
                      <div class=""mb-4"">
                        <span class=""inline-block px-4 py-2 bg-purple-100 text-purple-700 text-sm font-semibold rounded-full uppercase tracking-wider"">
                          Código de verificación
                        </span>
                      </div>
                      
                      <div class=""text-5xl font-black text-transparent bg-clip-text bg-gradient-to-r from-purple-600 to-indigo-600 tracking-widest mb-4 font-mono"">
                        847291
                      </div>
                      
                      <div class=""flex items-center justify-center gap-2 text-purple-600"">
                        <svg class=""w-4 h-4"" fill=""none"" stroke=""currentColor"" viewBox=""0 0 24 24"">
                          <path stroke-linecap=""round"" stroke-linejoin=""round"" stroke-width=""2"" d=""M12 8v4l3 3m6-3a9 9 0 11-18 0 9 9 0 0118 0z""/>
                        </svg>
                        <span class=""text-sm font-medium"">Expira en 10 minutos</span>
                      </div>
                    </div>
                  </div>

                  <!-- Instructions -->
                  <div class=""bg-gradient-to-r from-gray-50 to-purple-50 rounded-2xl p-8 mb-8 border border-gray-100"">
                    <div class=""flex items-start gap-4"">
                      <div class=""flex-shrink-0 w-8 h-8 bg-green-100 rounded-xl flex items-center justify-center"">
                        <svg class=""w-5 h-5 text-green-600"" fill=""none"" stroke=""currentColor"" viewBox=""0 0 24 24"">
                          <path stroke-linecap=""round"" stroke-linejoin=""round"" stroke-width=""2"" d=""M5 13l4 4L19 7""/>
                        </svg>
                      </div>
                      <div class=""flex-1"">
                        <h4 class=""text-lg font-bold text-gray-900 mb-3"">Pasos a seguir:</h4>
                        <div class=""space-y-3"">
                          <div class=""flex items-center gap-3"">
                            <span class=""flex-shrink-0 w-6 h-6 bg-purple-600 text-white text-xs font-bold rounded-full flex items-center justify-center"">1</span>
                            <span class=""text-gray-700"">Abre la aplicación trivo en tu dispositivo</span>
                          </div>
                          <div class=""flex items-center gap-3"">
                            <span class=""flex-shrink-0 w-6 h-6 bg-purple-600 text-white text-xs font-bold rounded-full flex items-center justify-center"">2</span>
                            <span class=""text-gray-700"">Ingresa el código de 6 dígitos</span>
                          </div>
                          <div class=""flex items-center gap-3"">
                            <span class=""flex-shrink-0 w-6 h-6 bg-purple-600 text-white text-xs font-bold rounded-full flex items-center justify-center"">3</span>
                            <span class=""text-gray-700"">¡Comienza tu aventura de conexiones!</span>
                          </div>
                        </div>
                      </div>
                    </div>
                  </div>

                  <!-- Security Alert -->
                  <div class=""bg-gradient-to-r from-amber-50 to-orange-50 border-l-4 border-amber-400 rounded-xl p-6 mb-8"">
                    <div class=""flex items-start gap-3"">
                      <div class=""flex-shrink-0 w-8 h-8 bg-amber-100 rounded-xl flex items-center justify-center"">
                        <svg class=""w-5 h-5 text-amber-600"" fill=""none"" stroke=""currentColor"" viewBox=""0 0 24 24"">
                          <path stroke-linecap=""round"" stroke-linejoin=""round"" stroke-width=""2"" d=""M12 9v2m0 4h.01m-6.938 4h13.856c1.54 0 2.502-1.667 1.732-2.5L13.732 4c-.77-.833-1.964-.833-2.732 0L3.732 16.5c-.77.833.192 2.5 1.732 2.5z""/>
                        </svg>
                      </div>
                      <div>
                        <h4 class=""font-bold text-amber-800 mb-2"">🔒 Protege tu cuenta</h4>
                        <p class=""text-amber-700 text-sm leading-relaxed"">
                          Este código es personal e intransferible. Nunca lo compartas con terceros. 
                          El equipo de trivo jamás te solicitará este código por teléfono o email.
                        </p>
                      </div>
                    </div>
                  </div>

                  <!-- CTA Button -->
                  <div class=""text-center mb-8"">
                    <a href=""#"" class=""inline-flex items-center gap-3 bg-gradient-to-r from-purple-600 via-purple-700 to-indigo-700 hover:from-purple-700 hover:via-purple-800 hover:to-indigo-800 text-white px-10 py-5 rounded-2xl font-bold text-lg shadow-xl hover:shadow-2xl transition-all duration-300 transform hover:scale-105 hover:-translate-y-1"">
                      <svg class=""w-6 h-6"" fill=""none"" stroke=""currentColor"" viewBox=""0 0 24 24"">
                        <path stroke-linecap=""round"" stroke-linejoin=""round"" stroke-width=""2"" d=""M10 6H6a2 2 0 00-2 2v10a2 2 0 002 2h10a2 2 0 002-2v-4M14 4h6m0 0v6m0-6L10 14""/>
                      </svg>
                      Abrir trivo ahora
                    </a>
                  </div>
                </div>

                <!-- Footer -->
                <div class=""bg-gradient-to-r from-gray-50 to-purple-50 px-8 py-8 border-t border-gray-100"">
                  <div class=""text-center space-y-6"">
                    
                    <!-- Help Text -->
                    <p class=""text-gray-600 text-sm"">
                      ¿No solicitaste este código? 
                      <a href=""#"" class=""text-purple-600 hover:text-purple-700 font-medium underline"">
                        Reporta actividad sospechosa
                      </a>
                    </p>
                    
                    <!-- Links -->
                    <div class=""flex items-center justify-center gap-8 text-sm"">
                      <a href=""#"" class=""text-gray-500 hover:text-purple-600 transition-colors font-medium"">
                        Centro de ayuda
                      </a>
                      <div class=""w-1 h-1 bg-gray-300 rounded-full""></div>
                      <a href=""#"" class=""text-gray-500 hover:text-purple-600 transition-colors font-medium"">
                        Soporte técnico
                      </a>
                      <div class=""w-1 h-1 bg-gray-300 rounded-full""></div>
                      <a href=""#"" class=""text-gray-500 hover:text-purple-600 transition-colors font-medium"">
                        Privacidad
                      </a>
                    </div>
                    
                    <!-- Copyright -->
                    <div class=""pt-6 border-t border-gray-200"">
                      <div class=""flex items-center justify-center gap-2 text-gray-400 text-xs"">
                        <svg class=""w-4 h-4"" fill=""currentColor"" viewBox=""0 0 24 24"">
                          <path d=""M20.84 4.61a5.5 5.5 0 0 0-7.78 0L12 5.67l-1.06-1.06a5.5 5.5 0 0 0-7.78 7.78l1.06 1.06L12 21.23l7.78-7.78 1.06-1.06a5.5 5.5 0 0 0 0-7.78z""/>
                        </svg>
                        <span>© 2024 trivo • Conectando corazones, creando historias</span>
                      </div>
                    </div>
                  </div>
                </div>
                
              </div>
              
              <!-- Bottom Spacing -->
              <div class=""h-8""></div>
              
            </div>

          </body>
          </html>";
    }

    public static string OlvidarContrasena(string code, string nombreUsuario)
    {
        return null;
    }
    
}