module.exports = {
    content: [
        'Pages/**/*.cshtml',     // Tüm Razor sayfalarını tara
        'wwwroot/js/**/*.js'     // JS dosyalarındaki olası class kullanımlarını da tara
    ],
    css: [
        'wwwroot/css/**/*.css'   // site.css dâhil tüm CSS dosyaları
    ],
    output: 'wwwroot/css/purged', // Sonuçları buraya kaydet
    safelist: [
        // Silinmesini istemediğin özel class’lar varsa buraya yaz (örnek:)
        // 'active', 'show', 'collapse'
    ]
}
