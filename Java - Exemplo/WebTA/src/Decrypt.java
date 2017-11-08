import br.com.bradesco.webta.security.crypto.WEBTACryptoUtil;
import br.com.bradesco.webta.security.crypto.WEBTAInputStream;
import br.com.bradesco.webta.security.exception.CryptoException;
import br.com.bradesco.webta.security.exception.ParameterException;

import java.io.*;

public class Decrypt {

    public static void main(String[] args) {
        String sourceFile = "",
                destinationFile = "",
                sourcePath ="",
                destinationPath = "",
                criptoPath = "",
                criptoPwd = "";

        try {
            if(args.length > 0) {
                sourceFile = args[0];
                destinationFile = args[1];
                sourcePath = args[2];
                destinationPath = args[3];
                criptoPath = args[4];
                criptoPwd = args[5];
            }
            else {
                System.out.println("Informe os Argumentos da Chamada do Arquivo");
            }
        }
        catch (ArrayIndexOutOfBoundsException e) {
            System.out.println("ArrayIndexOutOfBoundsException caught");
        }
        finally {
            ProcessaArquivo(sourceFile, destinationFile, sourcePath,destinationPath, criptoPath,criptoPwd);
        }
    }

    public static void ProcessaArquivo(String sourceFile, String destinationFile, String sourcePath, String destinationPath, String criptoPath,String criptoPwd)
    {
        //Instacia o metodo de Stream
        WEBTAInputStream wis = null;
        int bytesRead;
        byte[] bufDecripto = new byte[8196];

        try {
            byte[] chaveCripto = WEBTACryptoUtil.decodeKeyFile(new File(criptoPath), criptoPwd);

            wis = new WEBTAInputStream(sourceFile,sourcePath, chaveCripto);

           try(BufferedOutputStream bos = new BufferedOutputStream(new FileOutputStream(destinationPath + "/" + destinationFile))) {
                while ((bytesRead = wis.read(bufDecripto)) > 0) {
                    bos.write(bufDecripto, 0, bytesRead);
                    bos.flush();
                }

                bos.close();
            } catch (IOException e){
                System.out.println (e.getMessage());
            } finally {
                if (wis != null) {
                    try {
                        wis.close();
                    } catch (IOException e){
                        System.out.println (e.getMessage());
                    }
                }
            }
        } catch (ParameterException | IOException | CryptoException e){
            System.out.println (e.getMessage());
        } finally {
            if (wis != null) {
                try {
                    wis.close();
                } catch (IOException e){
                    System.out.println (e.getMessage());
                }
            }
        }
    }
}
