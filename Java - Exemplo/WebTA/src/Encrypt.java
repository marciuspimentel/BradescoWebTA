import br.com.bradesco.webta.security.crypto.WEBTACryptoUtil;
import br.com.bradesco.webta.security.crypto.WEBTAOutputStream;
import br.com.bradesco.webta.security.exception.CryptoException;
import br.com.bradesco.webta.security.exception.ParameterException;
import br.com.bradesco.webta.security.exception.GZipException;

import java.io.FileInputStream;
import java.io.InputStream;
import java.io.File;
import java.io.IOException;

/**
 * Created by marcius.pimentel on 05/09/2016.
 */
public class Encrypt {

    public static void main(String[] args) {
        String sourceFile = "",
                destinationFile = "",
                sourcePath ="",
                destinationPath = "",
                criptoPath = "",
                criptoPwd = "";


        if(args.length > 0) {
            sourceFile = args[0];
            destinationFile = args[1];
            sourcePath = args[2];
            destinationPath = args[3];
            criptoPath = args[4];
            criptoPwd = args[5];

            ProcessaArquivo(sourceFile, destinationFile, sourcePath,destinationPath, criptoPath,criptoPwd);
        }
        else {
            System.out.println("Informe os Argumentos da Chamada do Arquivo");
        }

    }

    public static void ProcessaArquivo(String sourceFile, String destinationFile, String sourcePath, String destinationPath, String criptoPath,String criptoPwd)
    {
        int bytesRead = 0;
        byte[] bufDecripto = new byte[8196];

        try {
            byte[] chaveCripto = WEBTACryptoUtil.decodeKeyFile(new File(criptoPath), criptoPwd);

            //Escreve os dados no Arquivo de Destino
            WEBTAOutputStream wis = new WEBTAOutputStream(destinationFile, destinationPath, chaveCripto);

            try {
                //Le os dados do Arquivo a Ser Criptografado
                InputStream inp = new FileInputStream(new File(sourcePath + "/" + sourceFile));

                while((bytesRead = inp.read(bufDecripto, 0, bufDecripto.length)) != -1)
                {
                    wis.write(bufDecripto, 0, bytesRead);
                }
                inp.close();
            } catch (IOException e){
            } finally {
                if (wis != null) {
                    try {
                        wis.close();
                    } catch (IOException e){
                    }
                }
            }
        } catch (ParameterException | IOException | CryptoException | GZipException e){
            System.out.println(e.getMessage());
        }
    }
}
