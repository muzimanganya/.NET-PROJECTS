﻿(function (e) { function t(e) { var t = parseInt(e, 10); if (isNaN(t)) { t = 0 } return Math.max(1, t) } function n(e, t, n) { var r = e - Math.floor(n.maxPageNumbers / 2); if (r <= 1) return 1; return Math.min(r, t - n.maxPageNumbers) } function r(e, t, n) { var r = e + Math.floor(n.maxPageNumbers / 2); if (r >= t) return t; return Math.max(r, 1 + n.maxPageNumbers) } function i(t, s, o) { s = Math.max(1, s); if (o.rowsPerPage > 0) { var u = t.find("tbody tr"); var a = Math.ceil(u.size() / o.rowsPerPage); if (o.autoHidePager && a <= 1) { e(o.pager).hide() } else if (a > 0) { s = Math.min(s, a); var f = (s - 1) * o.rowsPerPage; var l = s * o.rowsPerPage; e.each(u, function (t, n) { if (t >= f && t < l) { e(n).show() } else { e(n).hide() } }); var c = e(o.pager); c.find(o.currentPage).text(s); c.find(o.totalPages).text(a); var h = c.find(o.pageNumbers); if (h.size() > 0) { h.empty(); var p; var d = n(s, a, o); var v = r(s, a, o); for (var m = d; m <= v; m++) { p = s == m ? o.currentPageClass : ""; h.append("<a href='#' id='" + m + "' class='" + p + "'>" + m + "</a>") } h.children("a").click(function () { i(t, e(this).attr("id"), o); return false }) } var g = c.find(o.currentPage); var a = c.find(o.totalPages); var y = c.find(o.prevPage); var b = c.find(o.nextPage); y.show(); b.show(); if (g.text() == 1) { y.hide() } else if (g.text() == a.text()) { b.hide() } } } } e.fn.paginateTable = function (n) { var r = jQuery.extend({ rowsPerPage: 5, nextPage: ".nextPage", prevPage: ".prevPage", firstPage: ".firstPage", lastPage: ".lastPage", currentPage: ".currentPage", totalPages: ".totalPages", pageNumbers: ".pageNumbers", maxPageNumbers: 0, currentPageClass: "current", pager: ".pager", autoHidePager: true }, n || {}); return this.each(function () { var n = e(this); var s = e(r.pager); var o = s.find(r.nextPage); var u = s.find(r.prevPage); var a = s.find(r.currentPage).first(); if (a.size() > 0) { a.text("1") } else { s.append('<span style="display:none" class="' + r.currentPage.substr(1) + '" >1</span>'); a = s.find(r.currentPage).first() } var f = s.find(r.firstPage); var l = s.find(r.lastPage); o.unbind("click"); o.click(function () { var e = t(a.text()); i(n, e + 1, r); return false }); u.unbind("click"); u.click(function () { var e = t(a.text()); i(n, e - 1, r); return false }); f.unbind("click"); f.click(function () { i(n, 1, r); return false }); l.unbind("click"); l.click(function () { var e = n.find("tbody tr"); var t = Math.ceil(e.size() / r.rowsPerPage); i(n, t, r); return false }); i(n, t(a.text()), r) }) } })(jQuery)