# msdnsimport

Bu .NET paketi Microsoft DNS Servisinde tanımlı olan Zone'ları MaestroPanel'e aktarmanızı sağlar.

Desteklenen DNS tipleri: A, AAA, CNAME, MX, NS, TXT, SRV

## Download

* https://github.com/maestropanel/msdnsimport/releases

## Gereksinimler

* .NET Framework 4+
* Microsoft DNS Service
* MaestroPanel API Anahtarı

## Kullanım

msdnsimport aracı Windows işletim sistemi üzerinde komut satırından (command line) çalışmaktadır. Alabileceği parametreler aşağıdaki gibidir.


> __key:__ MaestroPanel API Key
>
> __host:__ MaestroPanel'e erişmek için kullanılan IP adresi veya Host ismi.
>
> __port:__ MaestroPanel'e erişmek için kullanılan Port numarası. Varsayılan 9715
>
> __ssl:__ MaestroPanel'e erişmek için kullanılan protokol. Varsayılan false

Örnek:

```
msdnsimport.exe --key MAESTROPANEL_API_KEY --host 10.2.5.6
```
