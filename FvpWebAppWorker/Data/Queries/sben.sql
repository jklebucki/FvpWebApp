SELECT DOKID, TYPDOK, KONTRAHID, CZYZPAR, KONTRAHNAZWA, ULICANR, KODPOCZTOWY, MIEJSCOWOSC, KODKRAJU, NIP, FIRMA, POWIAZANA, SPLITPAYMENT, NRDOK, DATASPRZEDAZY, DATA, NETTO, BRUTTO, VAT, 
	VATPROCENT_A, NETTO_A, BRUTTO_A, VAT_A, GTU_A,
    VATPROCENT_B, NETTO_B, BRUTTO_B, VAT_B, GTU_B, 
    VATPROCENT_C, NETTO_C, BRUTTO_C, VAT_C, GTU_C,  
    VATPROCENT_D, NETTO_D, BRUTTO_D, VAT_D, GTU_D, 
    VATPROCENT_E, NETTO_E, BRUTTO_E, VAT_E, GTU_E,
    VATPROCENT_F, NETTO_F, BRUTTO_F, VAT_F, GTU_F,
    VATPROCENT_Z, NETTO_Z, BRUTTO_Z, VAT_Z, GTU_Z,
    VATPROCENT_NP, NETTO_NP, BRUTTO_NP, VAT_NP, GTU_NP
FROM(
SELECT DOKNAGLID AS DOKID, 'FV' TYPDOK, KONTRAHID, CZYZPAR, KONTRAHNAZWA, ULICANR, KODPOCZTOWY, MIEJSCOWOSC, nvl(KODKRAJU,'-') AS KODKRAJU, nvl(NIP,'-') AS NIP, FIRMA, POWIAZANA, SPLITPAYMENT, NRDOK, DATASPRZEDAZY, DATA, NETTO, BRUTTO, VAT, 
	VATPROCENT_A, NETTO_A, BRUTTO_A, VAT_A, GTU_A,
    VATPROCENT_B, NETTO_B, BRUTTO_B, VAT_B, GTU_B, 
    VATPROCENT_C, NETTO_C, BRUTTO_C, VAT_C, GTU_C,  
    VATPROCENT_D, NETTO_D, BRUTTO_D, VAT_D, GTU_D, 
    VATPROCENT_E, NETTO_E, BRUTTO_E, VAT_E, GTU_E,
    VATPROCENT_F, NETTO_F, BRUTTO_F, VAT_F, GTU_F,
    VATPROCENT_Z, NETTO_Z, BRUTTO_Z, VAT_Z, GTU_Z,
    VATPROCENT_NP, NETTO_NP, BRUTTO_NP, VAT_NP, GTU_NP
FROM(
SELECT F.FVNAGLID AS DOKNAGLID, F.NRDOK AS NRDOK, nvl(KODKRAJU,'') AS KODKRAJU, TO_CHAR(F.Data,'yyyy-mm-dd HH24:MI:SS') DATA, ND.DATASPRZEDAZY, POWIAZANA, SPLITPAYMENT,
            F.KONTRAHID, F.KONTRAHNAZWA, nvl(F.KONTRAHNIP,'') AS NIP, nvl(KONTRAHKODPOCZT,'-') AS KODPOCZTOWY, nvl(KONTRAHMIEJSCOWOSC,'-') AS MIEJSCOWOSC, nvl(KONTRAHULICANR,'-') AS ULICANR, FAKTURAZPARAGONU AS CZYZPAR, nvl(FIRMA,-1) AS FIRMA,
            VATPROCENT_NP, GTU_NP,
            (S.BRUTTO_NP-S.VAT_NP) AS NETTO_NP, S.VAT_NP, S.BRUTTO_NP,
            VATPROCENT_Z, GTU_Z,
            (S.BRUTTO_Z-S.VAT_Z) AS NETTO_Z, S.VAT_Z, S.BRUTTO_Z,
            VATPROCENT_F, GTU_F,
            (S.BRUTTO_F-S.VAT_F) AS NETTO_F, S.VAT_F, S.BRUTTO_F,
            VATPROCENT_E, GTU_E,
            (S.BRUTTO_E-S.VAT_E) AS NETTO_E, S.VAT_E, S.BRUTTO_E,
            VATPROCENT_D, GTU_D,
            (S.BRUTTO_D-S.VAT_D) AS NETTO_D, S.VAT_D, S.BRUTTO_D,
            VATPROCENT_C, GTU_C,
            (S.BRUTTO_C-S.VAT_C) AS NETTO_C, S.VAT_C, S.BRUTTO_C,
            VATPROCENT_B, GTU_B,
            (S.BRUTTO_B-S.VAT_B) AS NETTO_B, S.VAT_B, S.BRUTTO_B,
            VATPROCENT_A, GTU_A,
            (S.BRUTTO_A-S.VAT_A) AS NETTO_A, S.VAT_A, S.BRUTTO_A,
            (S.BRUTTO_A-S.VAT_A)+(S.BRUTTO_B-S.VAT_B)+(S.BRUTTO_C-S.VAT_C)+(S.BRUTTO_D-S.VAT_D)+(S.BRUTTO_E-S.VAT_E)+(S.BRUTTO_F-S.VAT_F)+(S.BRUTTO_Z-S.VAT_Z)+(S.BRUTTO_NP-S.VAT_NP) NETTO,
            (S.VAT_A+S.VAT_B+S.VAT_C+S.VAT_D+S.VAT_E+S.VAT_F+S.VAT_Z+S.VAT_NP) VAT,
            (S.BRUTTO_A+S.BRUTTO_B+S.BRUTTO_C+S.BRUTTO_D+S.BRUTTO_E+S.BRUTTO_F+S.BRUTTO_Z+S.BRUTTO_NP) BRUTTO
        FROM FVNAGL F, 
        (SELECT FVNAGLID,
                sum(nvl(DECODE(VATSYMBOL,128,WARTOSCVAT),0)) AS VAT_NP,
                sum(nvl(DECODE(VATSYMBOL,128,WARTOSCBRUTTO),0)) AS BRUTTO_NP,
                sum(nvl(DECODE(VATSYMBOL,128,VATPROCENT),0)) AS VATPROCENT_NP,
                sum(nvl(DECODE(VATSYMBOL,64,WARTOSCVAT),0)) AS VAT_Z,
                sum(nvl(DECODE(VATSYMBOL,64,WARTOSCBRUTTO),0)) AS BRUTTO_Z,
                sum(nvl(DECODE(VATSYMBOL,64,VATPROCENT),0)) AS VATPROCENT_Z,
                sum(nvl(DECODE(VATSYMBOL,32,WARTOSCVAT),0)) AS VAT_F,
                sum(nvl(DECODE(VATSYMBOL,32,WARTOSCBRUTTO),0)) AS BRUTTO_F,
                sum(nvl(DECODE(VATSYMBOL,32,VATPROCENT),0)) AS VATPROCENT_F,
                sum(nvl(DECODE(VATSYMBOL,16,WARTOSCVAT),0)) AS VAT_E,
                sum(nvl(DECODE(VATSYMBOL,16,WARTOSCBRUTTO),0)) AS BRUTTO_E,
                sum(nvl(DECODE(VATSYMBOL,16,VATPROCENT),0)) AS VATPROCENT_E,
                sum(nvl(DECODE(VATSYMBOL,8,WARTOSCVAT),0)) AS VAT_D,
                sum(nvl(DECODE(VATSYMBOL,8,WARTOSCBRUTTO),0)) AS BRUTTO_D,
                sum(nvl(DECODE(VATSYMBOL,8,VATPROCENT),0)) AS VATPROCENT_D,
                sum(nvl(DECODE(VATSYMBOL,4,WARTOSCVAT),0)) AS VAT_C,
                sum(nvl(DECODE(VATSYMBOL,4,WARTOSCBRUTTO),0)) AS BRUTTO_C,
                sum(nvl(DECODE(VATSYMBOL,4,VATPROCENT),0)) AS VATPROCENT_C,
                sum(nvl(DECODE(VATSYMBOL,2,WARTOSCVAT),0)) AS VAT_B,
                sum(nvl(DECODE(VATSYMBOL,2,WARTOSCBRUTTO),0)) AS BRUTTO_B,
                sum(nvl(DECODE(VATSYMBOL,2,VATPROCENT),0)) AS VATPROCENT_B,
                sum(nvl(DECODE(VATSYMBOL,1,WARTOSCVAT),0)) AS VAT_A,
                sum(nvl(DECODE(VATSYMBOL,1,WARTOSCBRUTTO),0)) AS BRUTTO_A,
                sum(nvl(DECODE(VATSYMBOL,1,VATPROCENT),0)) AS VATPROCENT_A
            FROM FVSTAWKIVAT
            GROUP BY FVNAGLID) S,
            (SELECT ID, DATA, DATASPRZEDAZY, NRDOK FROM NAGLDOKROZCH) ND,
            (SELECT 
				FVNaglID,
				LISTAGG(DECODE(VATSYMBOL,1, GTU),'') WITHIN GROUP (ORDER BY FVNaglID) AS GTU_A,
				LISTAGG(DECODE(VATSYMBOL,2, GTU),'') WITHIN GROUP (ORDER BY FVNaglID) AS GTU_B,
			    LISTAGG(DECODE(VATSYMBOL,4, GTU),'') WITHIN GROUP (ORDER BY FVNaglID) AS GTU_C,
				LISTAGG(DECODE(VATSYMBOL,8, GTU),'') WITHIN GROUP (ORDER BY FVNaglID) AS GTU_D,
			    LISTAGG(DECODE(VATSYMBOL,16, GTU),'') WITHIN GROUP (ORDER BY FVNaglID) AS GTU_E,
				LISTAGG(DECODE(VATSYMBOL,32, GTU),'') WITHIN GROUP (ORDER BY FVNaglID) AS GTU_F,
				LISTAGG(DECODE(VATSYMBOL,64, GTU),'') WITHIN GROUP (ORDER BY FVNaglID) AS GTU_Z,
				LISTAGG(DECODE(VATSYMBOL,128, GTU),'') WITHIN GROUP (ORDER BY FVNaglID) AS GTU_NP
			FROM
			(select P.FVNaglID, P.VATSymbol, rtrim(ltrim(decode(nvl(sum(nvl(G.GTU1,0)),0),0,'','GTU_01,') || decode(nvl(sum(nvl(G.GTU2,0)),0),0,'','GTU_02,') || decode(nvl(sum(nvl(G.GTU3,0)),0),0,'','GTU_03,') ||
			        decode(nvl(sum(nvl(G.GTU4,0)),0),0,'','GTU_04,') || decode(nvl(sum(nvl(G.GTU5,0)),0),0,'','GTU_05,') || decode(nvl(sum(nvl(G.GTU6,0)),0),0,'','GTU_06,') ||
			        decode(nvl(sum(nvl(G.GTU7,0)),0),0,'','GTU_07,') || decode(nvl(sum(nvl(G.GTU8,0)),0),0,'','GTU_08,') || decode(nvl(sum(nvl(G.GTU9,0)),0),0,'','GTU_09,') ||
			        decode(nvl(sum(nvl(G.GTU10,0)),0),0,'','GTU_10,') || decode(nvl(sum(nvl(G.GTU11,0)),0),0,'','GTU_11,') || decode(nvl(sum(nvl(G.GTU12,0)),0),0,'','GTU_12,') ||
			        decode(nvl(sum(nvl(G.GTU13,0)),0),0,'','GTU_13,'),','),',') GTU
			 from GTU G, FVPoz P
			 where P.PozTowID = G.PozTowID (+)
			 group by P.FVNaglID, P.VATSymbol)
			 GROUP BY FVNaglID) VGTU
        WHERE  F.DATA BETWEEN TO_DATE('dataOd 00:00:00',  'YYYY-MM-DD HH24:MI:SS') AND TO_DATE('dataDo 23:59:59',  'YYYY-MM-DD HH24:MI:SS')
		AND F.FVNAGLID = S.FVNAGLID AND ND.ID=F.FVNAGLID AND F.FVNAGLID = VGTU.FVNaglID(+)
        ORDER BY F.DATA)
UNION ALL
SELECT DOKNAGLID AS DOKID, 'FVK' TYPDOK, KONTRAHID, CZYZPAR, KONTRAHNAZWA, ULICANR, KODPOCZTOWY, MIEJSCOWOSC, nvl(KODKRAJU,'-') AS KODKRAJU, nvl(NIP,'-') AS NIP, FIRMA, POWIAZANA, SPLITPAYMENT, NRDOK, DATASPRZEDAZY, DATA, NETTO, BRUTTO, VAT, 
	VATPROCENT_A, NETTO_A, BRUTTO_A, VAT_A, GTU_A,
    VATPROCENT_B, NETTO_B, BRUTTO_B, VAT_B, GTU_B, 
    VATPROCENT_C, NETTO_C, BRUTTO_C, VAT_C, GTU_C,  
    VATPROCENT_D, NETTO_D, BRUTTO_D, VAT_D, GTU_D, 
    VATPROCENT_E, NETTO_E, BRUTTO_E, VAT_E, GTU_E,
    VATPROCENT_F, NETTO_F, BRUTTO_F, VAT_F, GTU_F,
    VATPROCENT_Z, NETTO_Z, BRUTTO_Z, VAT_Z, GTU_Z,
    VATPROCENT_NP, NETTO_NP, BRUTTO_NP, VAT_NP, GTU_NP
FROM(
SELECT F.FVKNAGLID AS DOKNAGLID, F.NRDOK AS NRDOK, nvl(KODKRAJU,'') AS KODKRAJU, TO_CHAR(F.Data,'yyyy-mm-dd HH24:MI:SS') DATA, ND.DATASPRZEDAZY, POWIAZANA, SPLITPAYMENT,
            FV.KONTRAHID, FV.KONTRAHNAZWA, nvl(FV.KONTRAHNIP,'') AS NIP, nvl(KONTRAHKODPOCZT,'-') AS KODPOCZTOWY, nvl(KONTRAHMIEJSCOWOSC,'-') AS MIEJSCOWOSC, nvl(KONTRAHULICANR,'-') AS ULICANR, FAKTURAZPARAGONU AS CZYZPAR, nvl(FIRMA,-1) AS FIRMA,
            VATPROCENT_A, GTU_A,
            (S.BRUTTO_A-S.VAT_A) AS NETTO_A, S.VAT_A, S.BRUTTO_A,
            VATPROCENT_B, GTU_B,
            (S.BRUTTO_B-S.VAT_B) AS NETTO_B, S.VAT_B, S.BRUTTO_B,
            VATPROCENT_C, GTU_C,
            (S.BRUTTO_C-S.VAT_C) AS NETTO_C, S.VAT_C, S.BRUTTO_C,
            VATPROCENT_D, GTU_D,
            (S.BRUTTO_D-S.VAT_D) AS NETTO_D, S.VAT_D, S.BRUTTO_D,
            VATPROCENT_E, GTU_E,
            (S.BRUTTO_E-S.VAT_E) AS NETTO_E, S.VAT_E, S.BRUTTO_E,
            VATPROCENT_F, GTU_F,
            (S.BRUTTO_F-S.VAT_F) AS NETTO_F, S.VAT_F, S.BRUTTO_F,
            VATPROCENT_Z, GTU_Z,
            (S.BRUTTO_Z-S.VAT_Z) AS NETTO_Z, S.VAT_Z, S.BRUTTO_Z,
            VATPROCENT_NP, GTU_NP,
            (S.BRUTTO_NP-S.VAT_NP) AS NETTO_NP, S.VAT_NP, S.BRUTTO_NP,
            (S.BRUTTO_NP-S.VAT_NP)+(S.BRUTTO_C-S.VAT_C)+(S.BRUTTO_B-S.VAT_B)+(S.BRUTTO_A-S.VAT_A)+(S.BRUTTO_D-S.VAT_D)+(S.BRUTTO_E-S.VAT_E)+(S.BRUTTO_F-S.VAT_F)+(S.BRUTTO_Z-S.VAT_Z) NETTO,
            (S.VAT_A+S.VAT_B+S.VAT_C+S.VAT_D+S.VAT_E+S.VAT_F+S.VAT_Z+S.VAT_NP) VAT,
            (S.BRUTTO_A+S.BRUTTO_B+S.BRUTTO_C+S.BRUTTO_D+S.BRUTTO_E+S.BRUTTO_F+S.BRUTTO_Z+S.BRUTTO_NP) BRUTTO
        FROM FVKNAGL F,
            (SELECT FVKNAGLID,
                sum(nvl(DECODE(VATSYMBOL,128,WARTOSCVATPO-WARTOSCVATPRZED),0)) AS VAT_NP,
                sum(nvl(DECODE(VATSYMBOL,128,WARTOSCBRUTTOPO-WARTOSCBRUTTOPRZED),0)) AS BRUTTO_NP,
                sum(nvl(DECODE(VATSYMBOL,128,VATPROCENT),0)) AS VATPROCENT_NP,
                sum(nvl(DECODE(VATSYMBOL,64,WARTOSCVATPO-WARTOSCVATPRZED),0)) AS VAT_Z,
                sum(nvl(DECODE(VATSYMBOL,64,WARTOSCBRUTTOPO-WARTOSCBRUTTOPRZED),0)) AS BRUTTO_Z,
                sum(nvl(DECODE(VATSYMBOL,64,VATPROCENT),0)) AS VATPROCENT_Z,
                sum(nvl(DECODE(VATSYMBOL,32,WARTOSCVATPO-WARTOSCVATPRZED),0)) AS VAT_F,
                sum(nvl(DECODE(VATSYMBOL,32,WARTOSCBRUTTOPO-WARTOSCBRUTTOPRZED),0)) AS BRUTTO_F,
                sum(nvl(DECODE(VATSYMBOL,32,VATPROCENT),0)) AS VATPROCENT_F,
                sum(nvl(DECODE(VATSYMBOL,16,WARTOSCVATPO-WARTOSCVATPRZED),0)) AS VAT_E,
                sum(nvl(DECODE(VATSYMBOL,16,WARTOSCBRUTTOPO-WARTOSCBRUTTOPRZED),0)) AS BRUTTO_E,
                sum(nvl(DECODE(VATSYMBOL,16,VATPROCENT),0)) AS VATPROCENT_E,
                sum(nvl(DECODE(VATSYMBOL,8,WARTOSCVATPO-WARTOSCVATPRZED),0)) AS VAT_D,
                sum(nvl(DECODE(VATSYMBOL,8,WARTOSCBRUTTOPO-WARTOSCBRUTTOPRZED),0)) AS BRUTTO_D,
                sum(nvl(DECODE(VATSYMBOL,8,VATPROCENT),0)) AS VATPROCENT_D,
                sum(nvl(DECODE(VATSYMBOL,4,WARTOSCVATPO-WARTOSCVATPRZED),0)) AS VAT_C,
                sum(nvl(DECODE(VATSYMBOL,4,WARTOSCBRUTTOPO-WARTOSCBRUTTOPRZED),0)) AS BRUTTO_C,
                sum(nvl(DECODE(VATSYMBOL,4,VATPROCENT),0)) AS VATPROCENT_C,
                sum(nvl(DECODE(VATSYMBOL,2,WARTOSCVATPO-WARTOSCVATPRZED),0)) AS VAT_B,
                sum(nvl(DECODE(VATSYMBOL,2,WARTOSCBRUTTOPO-WARTOSCBRUTTOPRZED),0)) AS BRUTTO_B,
                sum(nvl(DECODE(VATSYMBOL,2,VATPROCENT),0)) AS VATPROCENT_B,
                sum(nvl(DECODE(VATSYMBOL,1,WARTOSCVATPO-WARTOSCVATPRZED),0)) AS VAT_A,
                sum(nvl(DECODE(VATSYMBOL,1,WARTOSCBRUTTOPO-WARTOSCBRUTTOPRZED),0)) AS BRUTTO_A,
                sum(nvl(DECODE(VATSYMBOL,1,VATPROCENT),0)) AS VATPROCENT_A
            FROM FVKSTAWKIVAT
            GROUP BY FVKNAGLID) S, 
            FVNAGL FV, (SELECT ID, DATA, DATASPRZEDAZY, NRDOK FROM NAGLDOKROZCH) ND,
            (SELECT 
				FVKNaglID,
				LISTAGG(DECODE(VATSYMBOL,1, GTU),'') WITHIN GROUP (ORDER BY FVKNaglID) AS GTU_A,
				LISTAGG(DECODE(VATSYMBOL,2, GTU),'') WITHIN GROUP (ORDER BY FVKNaglID) AS GTU_B,
			    LISTAGG(DECODE(VATSYMBOL,4, GTU),'') WITHIN GROUP (ORDER BY FVKNaglID) AS GTU_C,
				LISTAGG(DECODE(VATSYMBOL,8, GTU),'') WITHIN GROUP (ORDER BY FVKNaglID) AS GTU_D,
			    LISTAGG(DECODE(VATSYMBOL,16, GTU),'') WITHIN GROUP (ORDER BY FVKNaglID) AS GTU_E,
				LISTAGG(DECODE(VATSYMBOL,32, GTU),'') WITHIN GROUP (ORDER BY FVKNaglID) AS GTU_F,
				LISTAGG(DECODE(VATSYMBOL,64, GTU),'') WITHIN GROUP (ORDER BY FVKNaglID) AS GTU_Z,
				LISTAGG(DECODE(VATSYMBOL,128, GTU),'') WITHIN GROUP (ORDER BY FVKNaglID) AS GTU_NP
			FROM
			(select P.FVKNaglID, P.VATSymbol, rtrim(ltrim(decode(nvl(sum(nvl(G.GTU1,0)),0),0,'','GTU_01,') || decode(nvl(sum(nvl(G.GTU2,0)),0),0,'','GTU_02,') || decode(nvl(sum(nvl(G.GTU3,0)),0),0,'','GTU_03,') ||
			        decode(nvl(sum(nvl(G.GTU4,0)),0),0,'','GTU_04,') || decode(nvl(sum(nvl(G.GTU5,0)),0),0,'','GTU_05,') || decode(nvl(sum(nvl(G.GTU6,0)),0),0,'','GTU_06,') ||
			        decode(nvl(sum(nvl(G.GTU7,0)),0),0,'','GTU_07,') || decode(nvl(sum(nvl(G.GTU8,0)),0),0,'','GTU_08,') || decode(nvl(sum(nvl(G.GTU9,0)),0),0,'','GTU_09,') ||
			        decode(nvl(sum(nvl(G.GTU10,0)),0),0,'','GTU_10,') || decode(nvl(sum(nvl(G.GTU11,0)),0),0,'','GTU_11,') || decode(nvl(sum(nvl(G.GTU12,0)),0),0,'','GTU_12,') ||
			        decode(nvl(sum(nvl(G.GTU13,0)),0),0,'','GTU_13,'),','),',') GTU
			 from GTU G, FVKPoz P
			 where P.PozTowID = G.PozTowID (+)
			 group by P.FVKNaglID, P.VATSymbol)
			 GROUP BY FVKNaglID) VGTU
        WHERE  F.DATA BETWEEN TO_DATE('dataOd 00:00:00',  'YYYY-MM-DD HH24:MI:SS') AND TO_DATE('dataDo 23:59:59',  'YYYY-MM-DD HH24:MI:SS')
            AND F.FVKNAGLID = S.FVKNAGLID AND F.DOTYCZYDOKID = FV.FVNAGLID AND ND.ID=F.FVKNAGLID AND F.FVKNAGLID = VGTU.FVKNAGLID (+)
ORDER BY F.DATA))