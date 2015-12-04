<%@ Page Title="Slide" Language="C#" AutoEventWireup="true" CodeBehind="Slide.aspx.cs" Inherits="WebApplication1_revealjs.Slide" %>
<!doctype html>
<html lang="en">

	<head>
		<meta charset="utf-8">

		<title>Arirang News Slide Viewer</title>

		<meta name="description" content="Arirang News Slide Viewer using reveal.js">
		<meta name="author" content="crowdy">
        <meta name=viewport content="width=device-width, initial-scale=1">
		<meta name="apple-mobile-web-app-capable" content="yes" />
		<meta name="apple-mobile-web-app-status-bar-style" content="black-translucent" />

		<link rel="stylesheet" href="/css/reveal.css">
		<link rel="stylesheet" href="/css/theme/default.css" id="theme">

		<!-- For syntax highlighting -->
		<link rel="stylesheet" href="/lib/css/zenburn.css">

		<!-- If the query includes 'print-pdf', use the PDF print sheet -->
		<script>
			document.write( '<link rel="stylesheet" href="/css/print/' + ( window.location.search.match( /print-pdf/gi ) ? 'pdf' : 'paper' ) + '.css" type="text/css" media="print">' );
		</script>

		<!--[if lt IE 9]>
		<script src="lib/js/html5shiv.js"></script>
		<![endif]-->
	</head>

	<body>

		<div class="reveal">

			<!-- Any section element inside of this container is displayed as a slide -->
			<div class="slides">

                <asp:Literal runat="server" ID="ltContent"></asp:Literal>

			</div>

		</div>

		<script src="/lib/js/head.min.js"></script>
		<script src="/js/reveal.min.js"></script>

		<script>
			// Full list of configuration options available here:
			// https://github.com/hakimel/reveal.js#configuration
			Reveal.initialize({
			    controls: true, // 右下のコントロールを非表示
			    progress: true, // 下部の進捗バーを非表示
			    history: true, // 各ページをブラウザの履歴に残す

			    slideNumber: false, // 右下にスライドのページ番号を表示
			    overview: false, // Escで表示できる「概要」ページを表示不可にする
			    center: false, // スライドの垂直方向中央表示をしない（上寄せになる）
			    touch: true, // タッチ操作を無効化
			    loop: false, // スライドをループさせる
			    rtl: false,  // 右から左へのスライド
			    fragments: true, // fragmentsを無効化する
			    autoSlide: 0, // 自動スライドをミリ秒単位で設定。デフォルトは0（自動スライドしない）
			    autoSlideStoppable: true, // 左下の自動スライド停止用のボタンを非表示にする
			    mouseWheel: true, // マウスのホイールによるスライドを可能にする
			    transitionSpeed: 'default',  // スライド速度を遅く設定。 設定値 default/fast/slow
			    rollingLinks: false,

				theme: Reveal.getQueryHash().theme, // available themes are in /css/theme
				// transition: Reveal.getQueryHash().transition || 'default', // default/cube/page/concave/zoom/linear/none
				transition: 'linear',

			    // 視差背景の画像URL設定
				// parallaxBackgroundImage: 'https://s3.amazonaws.com/hakim-static/reveal-js/reveal-parallax-1.jpg',
			    // 視差背景のサイズを設定
				//parallaxBackgroundSize: '2100px 1024x',

				// Optional libraries used to extend on reveal.js
				dependencies: [
					{ src: '/lib/js/classList.js', condition: function() { return !document.body.classList; } },
					{ src: '/plugin/markdown/showdown.js', condition: function() { return !!document.querySelector( '[data-markdown]' ); } },
					{ src: '/plugin/markdown/markdown.js', condition: function() { return !!document.querySelector( '[data-markdown]' ); } },
					{ src: '/plugin/highlight/highlight.js', async: true, callback: function() { hljs.initHighlightingOnLoad(); } },
					{ src: '/plugin/zoom-js/zoom.js', async: true, condition: function() { return !!document.body.classList; } },
					{ src: '/plugin/notes/notes.js', async: true, condition: function() { return !!document.body.classList; } }
				]
			});

		</script>

	</body>
</html>
