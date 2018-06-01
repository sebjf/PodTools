import os
import io_scene_bl4.pbdf as pbdf

orig_file_path = r"D:\Archive\Games\Pod\Installation\Data\Binary\Circuits\Beltane.bl4"
dec_file_path = r"D:\Pictures\Beltane.dec.bl4"
enc_file_path = r"D:\Pictures\Beltane.enc.bl4"

# Decrypt data
with open(orig_file_path, "rb") as orig_file:
    with open(dec_file_path, "wb") as  dec_file:
        file_size = os.path.getsize(orig_file_path)
        key = pbdf.retrieve_key(orig_file, file_size)
        orig_file.seek(0)

        block_size = pbdf.retrieve_block_size(orig_file, file_size, key)
        orig_file.seek(0)

        pbdf.decrypt(orig_file, dec_file, key, block_size)

# Re-encrypt data
with open(dec_file_path, "rb") as dec_file:
    with open(enc_file_path, "wb") as enc_file:
        pbdf.encrypt(dec_file, enc_file, key, block_size)
